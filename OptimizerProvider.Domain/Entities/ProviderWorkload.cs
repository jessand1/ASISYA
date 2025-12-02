using System;


namespace ProviderOptimizer.Domain.Entities
{
    public class ProviderWorkload
    {
        public Guid ProviderId { get; set; }
        public int ActiveCases { get; set; } = 0;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Provider? Provider { get; set; }
    }
}