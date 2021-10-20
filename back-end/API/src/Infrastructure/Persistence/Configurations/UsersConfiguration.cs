using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    public class UsersConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users", "project");

            builder.HasKey(e => e.Id)
                    .HasName("pk_user");

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.Name)
                .HasColumnName("name");

            builder.Property(e => e.Surname)
                .HasColumnName("surname");

            builder.Property(e => e.PasswordHash)
                .HasColumnName("password_hash");

            builder.Property(e => e.Username)
                .HasColumnName("username");

            builder.Property(e => e.CreatedAt)
                .HasColumnName("created_at");
        }
    }
}
