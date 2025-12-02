using MediatR;
using OptimizerProvider.Application.Features.Provider.DTOs;

namespace OptimizerProvider.Application.Features.Provider.Queries
{
    public class GetProviderWorkloadQuery : IRequest<ProviderWorkloadDto?>
    {
        public Guid ProviderId { get; }

        public GetProviderWorkloadQuery(Guid providerId)
        {
            ProviderId = providerId;
        }
    }
}
