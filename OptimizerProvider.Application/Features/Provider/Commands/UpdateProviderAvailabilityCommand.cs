using MediatR;

namespace ProviderOptimizer.Application.Provider.Commands
{
    public class UpdateProviderAvailabilityCommand : IRequest<bool>
    {
        public Guid ProviderId { get; set; }
        public bool IsAvailable { get; set; }
        public string? Status { get; set; } 
    }
}
