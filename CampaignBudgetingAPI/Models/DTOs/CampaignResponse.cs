namespace CampaignBudgetingAPI.Api.DTOs.Responses
{
    public class CampaignResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public CampaignUserResponse User { get; private set; }
        public CampaignClientResponse Client { get; private set; }


        public record CampaignUserResponse
        {
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;

        }

        public record CampaignClientResponse
        {
            public string Name { get; set; }
            public decimal CommissionRate { get; set; }

        }
    }
}
