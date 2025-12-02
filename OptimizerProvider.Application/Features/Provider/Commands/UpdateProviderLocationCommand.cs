using MediatR;

namespace ProviderOptimizer.Application.Provider.Commands
{
    public class UpdateProviderLocationCommand : IRequest<bool>
    {
        public Guid ProviderId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
