using System;
using System.ComponentModel.DataAnnotations;

namespace CampaignBudgetingAPI.Api.DTOs.Requests
{
    public class CampaignUpdateRequest
    {
        [Required]
        public Guid Id { get; set; }

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
