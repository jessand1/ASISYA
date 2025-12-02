using System;


namespace ProviderOptimizer.Domain.Entities
{
    public class OptimizationRequest
    {
        public Guid RequestId { get; set; }
        public decimal ClientLatitude { get; set; }
        public decimal ClientLongitude { get; set; }
        public string ServiceType { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<OptimizationResult>? Results { get; set; }
    }
}