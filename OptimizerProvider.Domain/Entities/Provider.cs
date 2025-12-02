using System;
using System.Collections.Generic;


namespace ProviderOptimizer.Domain.Entities
{
    public class Provider
    {
        public Guid ProviderId { get; set; }
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string ServiceTypes { get; set; } = null!; 
        public decimal Rating { get; set; } = 5.0m;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ProviderAvailability? Availability { get; set; }
        public ProviderLocation? Location { get; set; }
        public ProviderWorkload? Workload { get; set; }
        public ICollection<OptimizationResult>? OptimizationResults { get; set; }
    }
}