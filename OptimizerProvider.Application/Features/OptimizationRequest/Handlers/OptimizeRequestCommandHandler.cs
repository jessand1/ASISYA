using AutoMapper;
using MediatR;
using ProviderOptimizer.Application.Interfaces;
using ProviderOptimizer.Application.Optimization.Commands;
using ProviderOptimizer.Application.Optimization.DTOs;
using ProviderOptimizer.Domain.Entities;
using ProviderOptimizer.Domain.Interfaces;

namespace ProviderOptimizer.Application.Optimization.Handlers
{
    public class OptimizeRequestCommandHandler
        : IRequestHandler<OptimizeRequestCommand, OptimizationResultDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OptimizeRequestCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OptimizationResultDto> Handle(
            OptimizeRequestCommand request,
            CancellationToken cancellationToken)
        {
            // -----------------------------------------
            // 1. Crear registro OptimizationRequest
            // -----------------------------------------
            var optimizationRequest = new OptimizationRequest
            {
                RequestId = Guid.NewGuid(),
                ClientLatitude = request.ClientLatitude,
                ClientLongitude = request.ClientLongitude,
                ServiceType = request.ServiceType
            };

            await _unitOfWork.Repository<OptimizationRequest>().AddAsync(optimizationRequest);

            // -----------------------------------------
            // 2. Obtener proveedores disponibles
            // -----------------------------------------
            var providerRepo = _unitOfWork.Repository<ProviderOptimizer.Domain.Entities.Provider>();

            var allProviders = await providerRepo.GetAllAsync();

            var candidates = allProviders
                .Where(p => p.IsActive
                            && p.Availability != null
                            && p.Availability.IsAvailable
                            && p.ServiceTypes.Contains(request.ServiceType))
                .ToList();

            if (!candidates.Any())
                throw new Exception("No providers available.");

            // -----------------------------------------
            // 3. Calcular resultados por proveedor
            // -----------------------------------------
            var results = new List<OptimizationResult>();

            foreach (var provider in candidates)
            {
                var distanceKm = CalculateDistance(
                    request.ClientLatitude,
                    request.ClientLongitude,
                    provider.Location?.Latitude ?? 0,
                    provider.Location?.Longitude ?? 0);

                var etaMin = CalculateEta(distanceKm);
                var score = CalculateScore(distanceKm, etaMin, provider.Rating);

                var optimizationResult = new OptimizationResult
                {
                    ResultId = Guid.NewGuid(),
                    RequestId = optimizationRequest.RequestId,
                    ProviderId = provider.ProviderId,
                    DistanceKm = distanceKm,
                    EtaMinutes = etaMin,
                    Score = score
                };

                results.Add(optimizationResult);

                await _unitOfWork.Repository<OptimizationResult>().AddAsync(optimizationResult);
            }

            // -----------------------------------------
            // 4. Guardar en DB
            // -----------------------------------------
            await _unitOfWork.SaveAsync();

            // -----------------------------------------
            // 5. Seleccionar el mejor proveedor
            // -----------------------------------------
            var best = results.OrderByDescending(r => r.Score).First();

            return _mapper.Map<OptimizationResultDto>(best);
        }

        // -----------------------------------------
        // Helpers: distancia, ETA, score
        // -----------------------------------------

        private decimal CalculateDistance(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
        {
            const double R = 6371; // radio tierra km

            var dLat = (double)(lat2 - lat1) * Math.PI / 180;
            var dLon = (double)(lon2 - lon1) * Math.PI / 180;

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos((double)lat1 * Math.PI / 180) *
                    Math.Cos((double)lat2 * Math.PI / 180) *
                    Math.Sin(dLon / 2) *
                    Math.Sin(dLon / 2);

            var c = 2 * Math.Asin(Math.Sqrt(a));
            return (decimal)(R * c);
        }

        private int CalculateEta(decimal distanceKm)
        {
            if (distanceKm <= 0) return 5;
            return (int)Math.Ceiling((double)distanceKm * 2.5) + 5;
        }

        private decimal CalculateScore(decimal distanceKm, int eta, decimal rating)
        {
            return (rating * 2) + (100m / (1 + distanceKm + eta));
        }
    }
}
