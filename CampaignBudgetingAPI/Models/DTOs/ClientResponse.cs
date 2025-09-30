using System;

namespace CampaignBudgetingAPI.Models.DTOs
{
    public class ClientResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal CommissionRate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
