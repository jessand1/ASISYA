using MediatR;
using OptimizerProvider.Application.Features.Provider.DTOs;
namespace ProviderOptimizer.Application.Provider.Queries
{
    public class GetAvailableProvidersQuery : IRequest<IEnumerable<ProviderDto>>
    {
    }
}
