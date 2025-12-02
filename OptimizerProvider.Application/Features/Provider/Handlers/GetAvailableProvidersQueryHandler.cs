using AutoMapper;
using MediatR;
using OptimizerProvider.Application.Features.Provider.DTOs;
using ProviderOptimizer.Application.Interfaces;
using ProviderOptimizer.Application.Provider.Queries;

namespace ProviderOptimizer.Application.Provider.Handlers
{
    public class GetAvailableProvidersQueryHandler
        : IRequestHandler<GetAvailableProvidersQuery, IEnumerable<ProviderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAvailableProvidersQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProviderDto>> Handle(
            GetAvailableProvidersQuery request,
            CancellationToken cancellationToken)
        {
            var providerRepo = _unitOfWork.Repository<ProviderOptimizer.Domain.Entities.Provider>();

            // Obtener proveedores activos + disponibles
            var providers = await providerRepo.GetAllAsync();

            var availableProviders = providers
                .Where(p => p.IsActive &&
                            p.Availability != null &&
                            p.Availability.IsAvailable)
                .ToList();

            // Mapear manualmente porque tus entidades son anidadas
            var result = providers.Select(p => new ProviderDto
            {
                ProviderId = p.ProviderId,
                Name = p.Name,
                Rating = p.Rating,
                ServiceTypes = p.ServiceTypes,
                IsAvailable = p.Availability!.IsAvailable,
                Latitude = p.Location?.Latitude ?? 0,
                Longitude = p.Location?.Longitude ?? 0,
                ActiveCases = p.Workload?.ActiveCases ?? 0
            });

            return result;
        }
    }
}
