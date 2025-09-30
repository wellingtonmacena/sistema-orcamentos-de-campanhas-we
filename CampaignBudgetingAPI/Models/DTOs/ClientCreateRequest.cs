using System.ComponentModel.DataAnnotations;

namespace CampaignBudgetingAPI.Models.DTOs
{
    public class ClientCreateRequest
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal CommissionRate { get; set; }
    }
}
