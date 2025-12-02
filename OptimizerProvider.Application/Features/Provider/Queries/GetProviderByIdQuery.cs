using MediatR;
using OptimizerProvider.Application.Features.Provider.DTOs;

namespace ProviderOptimizer.Application.Provider.Queries
{
    public class GetProviderByIdQuery : IRequest<ProviderDto?>
    {
        public Guid ProviderId { get; }

        public GetProviderByIdQuery(Guid providerId)
        {
            ProviderId = providerId;
        }
    }
}
