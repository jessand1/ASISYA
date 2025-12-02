using MediatR;
using OptimizerProvider.Application.Features.Provider.DTOs;
using OptimizerProvider.Application.Features.Provider.Queries;
using ProviderOptimizer.Application.Interfaces;
using ProviderOptimizer.Application.Provider.Queries;
using ProviderOptimizer.Domain.Entities;
using ProviderOptimizer.Domain.Interfaces;

namespace OptimizerProvider.Application.Features.Provider.Handlers
{
    public class GetProviderWorkloadQueryHandler
        : IRequestHandler<GetProviderWorkloadQuery, ProviderWorkloadDto?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProviderWorkloadQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProviderWorkloadDto?> Handle(
            GetProviderWorkloadQuery request,
            CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<ProviderWorkload>();

            // Workload usa ProviderId como PK
            var workload = await repo.GetByIdAsync(request.ProviderId);

            if (workload is null)
                return null;

            return new ProviderWorkloadDto
            {
                ProviderId = workload.ProviderId,
                ActiveCases = workload.ActiveCases,
                UpdatedAt = workload.UpdatedAt
            };
        }
    }
}
