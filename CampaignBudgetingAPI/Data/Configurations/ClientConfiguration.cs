using CampaignBudgetingAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampaignBudgetingAPI.Data.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            // Nome da tabela
            builder.ToTable("clients");

            // Chave primária
            builder.HasKey(c => c.Id);

            // Colunas
            builder.Property(c => c.Id)
                   .HasColumnName("id")
                   .IsRequired();

            builder.Property(c => c.CreatedAt)
                   .HasColumnName("created_at")
                   .IsRequired();

            builder.Property(c => c.UpdatedAt)
                   .HasColumnName("updated_at")
                   .IsRequired();

            builder.Property(c => c.Name)
                   .HasColumnName("name")
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(c => c.CommissionRate)
                   .HasColumnName("commission_rate")
                   .HasColumnType("numeric(5,2)")
                   .IsRequired();

            // Relação com Campaigns
            builder.HasMany(c => c.Campaigns)
                   .WithOne(ca => ca.Client)
                   .HasForeignKey(ca => ca.ClientId)
                   .HasConstraintName("fk_campaigns_client");
        }
    }
}
