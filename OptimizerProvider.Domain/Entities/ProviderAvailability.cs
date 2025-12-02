using System;


namespace ProviderOptimizer.Domain.Entities
{
    public class ProviderAvailability
    {
        public Guid ProviderId { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string Status { get; set; } = "AVAILABLE";
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
        public Provider? Provider { get; set; }
    }
}