using CampaignBudgetingAPI.Models.ValueObjects;

namespace CampaignBudgetingAPI.Models.Entities;

public class User : Entity
{
    private User() { }

    public string FullName { get; set; }
    public string Email { get; set; }
    public UserType UserType { get; set; }
    public string Password { get; set; }

    public ICollection<Campaign> Campaigns { get; private set; } = new List<Campaign>();

    public User(string fullName, string email, UserType userType, string password)
    {
        FullName = fullName;
        Email = email;
        UserType = userType;
        Password = password;
    }
}
