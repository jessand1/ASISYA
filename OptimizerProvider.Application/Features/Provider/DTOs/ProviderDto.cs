namespace OptimizerProvider.Application.Features.Provider.DTOs
{
    public class ProviderDto
    {
        public Guid ProviderId { get; set; }
        public string Name { get; set; } = null!;
        public string ServiceTypes { get; set; } = null!;
        public decimal Rating { get; set; }

        public bool IsAvailable { get; set; }
        public DateTime AvailabilityUpdatedAt { get; set; }

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime LocationUpdatedAt { get; set; }

        public int ActiveCases { get; set; }
    }
}
