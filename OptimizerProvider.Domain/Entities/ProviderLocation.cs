using System;


namespace ProviderOptimizer.Domain.Entities
{
    public class ProviderLocation
    {
        public Guid ProviderId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Provider? Provider { get; set; }
    }
}