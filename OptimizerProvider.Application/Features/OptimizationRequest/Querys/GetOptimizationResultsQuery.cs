using MediatR;
using ProviderOptimizer.Application.Optimization.DTOs;

namespace ProviderOptimizer.Application.Optimization.Queries
{
    public class GetOptimizationResultsQuery : IRequest<IEnumerable<OptimizationResultDto>>
    {
        public Guid RequestId { get; }

        public GetOptimizationResultsQuery(Guid requestId)
        {
            RequestId = requestId;
        }
    }
}
