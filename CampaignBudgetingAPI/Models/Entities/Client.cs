namespace CampaignBudgetingAPI.Models.Entities
{
    public class Client : Entity
    {
        public string Name { get; private set; }
        public decimal CommissionRate { get; private set; }

        // Relação com Campaigns
        public ICollection<Campaign> Campaigns { get; private set; } = new List<Campaign>();

        private Client() { } // EF Core

        public Client(string name, decimal commissionRate)
        {
            Name = name;
            CommissionRate = commissionRate;
        }

        public void Update(string name, decimal commissionRate)
        {
            Name = name;
            CommissionRate = commissionRate;
            MarkAsUpdated();
        }
    }
}
