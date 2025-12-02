using MediatR;
using ProviderOptimizer.Application.Optimization.DTOs;

namespace ProviderOptimizer.Application.Optimization.Commands
{
    public class OptimizeRequestCommand : IRequest<OptimizationResultDto>
    {
        public decimal ClientLatitude { get; set; }
        public decimal ClientLongitude { get; set; }
        public string ServiceType { get; set; } = null!;
    }
}
