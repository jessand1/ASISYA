namespace OptimizerProvider.Application.Features.Provider.DTOs
{
    public class ProviderWorkloadDto
    {
        public Guid ProviderId { get; set; }
        public int ActiveCases { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
