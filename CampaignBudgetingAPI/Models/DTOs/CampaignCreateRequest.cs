using System;
using System.ComponentModel.DataAnnotations;

namespace CampaignBudgetingAPI.Models.DTOs
{
    public class CampaignCreateRequest
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        public Guid ClientId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }
}
