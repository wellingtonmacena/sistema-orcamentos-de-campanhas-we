using CampaignBudgetingAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampaignBudgetingAPI.Data.Configurations
{
    public class CampaignConfiguration : IEntityTypeConfiguration<Campaign>
    {
        public void Configure(EntityTypeBuilder<Campaign> builder)
        {
            // Nome da tabela
            builder.ToTable("campaigns");

            // Chave primária
            builder.HasKey(c => c.Id);

            // Colunas
            builder.Property(c => c.Id)
                   .HasColumnName("id")
                   .IsRequired();

            builder.Property(c => c.Name)
                   .HasColumnName("name")
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(c => c.ClientId)
                   .HasColumnName("client_id")
                   .IsRequired();

            builder.Property(c => c.Status)
                   .HasColumnName("status")
                   .IsRequired();

            builder.Property(c => c.CreatedAt)
                   .HasColumnName("created_at")
                   .IsRequired();

            builder.Property(c => c.UpdatedAt)
                   .HasColumnName("updated_at")
                   .IsRequired();

            builder.Property(c => c.UserId)
                   .HasColumnName("user_id")
                   .IsRequired();

            // Relações (FKs)
            builder.HasOne(c => c.User)
                   .WithMany(u => u.Campaigns)
                   .HasForeignKey(c => c.UserId)
                   .HasConstraintName("fk_campaigns_user");

            builder.HasOne(c => c.Client)
                   .WithMany(cl => cl.Campaigns)
                   .HasForeignKey(c => c.ClientId)
                   .HasConstraintName("fk_campaigns_client");
        }
    }
}
