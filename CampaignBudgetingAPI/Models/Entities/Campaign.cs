using System;

namespace CampaignBudgetingAPI.Models.Entities
{
    public class Campaign : Entity
    {
        public string Name { get; private set; }
        public Guid ClientId { get; private set; }
        public string Status { get; private set; }
        public Guid UserId { get; private set; }

        // 🔗 Navegação (FKs)
        public User User { get; private set; }
        public Client Client { get; private set; }

        private Campaign() { } // EF Core

        public Campaign(string name, Guid clientId, string status, Guid userId)
        {
            Name = name;
            ClientId = clientId;
            Status = status;
            UserId = userId;
        }

        public void Update(string name, string status, Guid clientId)
        {
            Name = name;
            Status = status;
            ClientId = clientId;
            MarkAsUpdated();
        }
    }
}
