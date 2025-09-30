namespace CampaignBudgetingAPI.Models.Entities;

public class Entity
{
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    public void MarkAsUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
