using System;


namespace ProviderOptimizer.Domain.Entities
{
    public class OptimizationResult
    {
        public Guid ResultId { get; set; }
        public Guid RequestId { get; set; }
        public Guid ProviderId { get; set; }
        public int EtaMinutes { get; set; }
        public decimal DistanceKm { get; set; }
        public decimal Score { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public OptimizationRequest? Request { get; set; }
        public Provider? Provider { get; set; }
    }
}