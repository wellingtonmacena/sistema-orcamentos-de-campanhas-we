namespace CampaignBudgetingAPI.Data.Configurations
{
    using global::CampaignBudgetingAPI.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    namespace CampaignBudgetingAPI.Data.Configurations
    {
        public class UserConfiguration : IEntityTypeConfiguration<User>
        {
            public void Configure(EntityTypeBuilder<User> builder)
            {
                // Nome da tabela
                builder.ToTable("users");


                // Chave primária
                builder.HasKey(u => u.Id);

                // Colunas
                builder.Property(u => u.Id)
                       .HasColumnName("id")
                       .IsRequired();

                builder.Property(u => u.CreatedAt)
                       .HasColumnName("created_at")
                       .IsRequired();

                builder.Property(u => u.UpdatedAt)
                       .HasColumnName("updated_at")
                       .IsRequired();

                builder.Property(u => u.FullName)
                       .HasColumnName("full_name")
                       .IsRequired()
                       .HasMaxLength(200);

                builder.Property(u => u.Email)
                       .HasColumnName("email")
                       .IsRequired()
                       .HasMaxLength(150);

                builder.HasIndex(u => u.Email)
                       .IsUnique();

                builder.Property(u => u.UserType)
                       .HasColumnName("employee_role")
                       .HasConversion<string>() // salva como texto, igual ao SQL
                       .IsRequired();

                builder.Property(u => u.Password)
                       .HasColumnName("password")
                       .IsRequired()
                       .HasMaxLength(255);
            }
        }
    }

}
