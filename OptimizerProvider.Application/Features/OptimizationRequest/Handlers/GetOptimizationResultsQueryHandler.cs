using AutoMapper;
using MediatR;
using ProviderOptimizer.Application.Interfaces;
using ProviderOptimizer.Application.Optimization.DTOs;
using ProviderOptimizer.Application.Optimization.Queries;
using ProviderOptimizer.Domain.Entities;
using ProviderOptimizer.Domain.Interfaces;

namespace ProviderOptimizer.Application.Optimization.Handlers
{
    public class GetOptimizationResultsQueryHandler
        : IRequestHandler<GetOptimizationResultsQuery, IEnumerable<OptimizationResultDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOptimizationResultsQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OptimizationResultDto>> Handle(
            GetOptimizationResultsQuery request,
            CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<OptimizationResult>();

            var results = await repo.FindAsync(r => r.RequestId == request.RequestId);

            return _mapper.Map<IEnumerable<OptimizationResultDto>>(results);
        }
    }
}
