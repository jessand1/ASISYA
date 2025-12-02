using AutoMapper;
using MediatR;
using OptimizerProvider.Application.Features.Provider.DTOs;
using ProviderOptimizer.Application.Interfaces;
using ProviderOptimizer.Application.Provider.Queries;
using ProviderOptimizer.Domain.Entities;
using ProviderOptimizer.Domain.Interfaces;

namespace ProviderOptimizer.Application.Provider.Handlers
{
    public class GetProviderByIdQueryHandler
        : IRequestHandler<GetProviderByIdQuery, ProviderDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProviderByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProviderDto?> Handle(
            GetProviderByIdQuery request,
            CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<ProviderOptimizer.Domain.Entities.Provider>();

            var provider = await repo.GetByIdAsync(request.ProviderId);

            if (provider is null)
                return null;

            return new ProviderDto
            {
                ProviderId = provider.ProviderId,
                Name = provider.Name,
                Rating = provider.Rating,
                ServiceTypes = provider.ServiceTypes,
                IsAvailable = provider.Availability?.IsAvailable ?? false,
                Latitude = provider.Location?.Latitude ?? 0,
                Longitude = provider.Location?.Longitude ?? 0,
                ActiveCases = provider.Workload?.ActiveCases ?? 0
            };
        }
    }
}
