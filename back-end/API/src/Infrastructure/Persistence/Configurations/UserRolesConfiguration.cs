using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    public class UserRolesConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("user_roles", "project");

            builder.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("pk_user_role");

            builder.Property(e => e.UserId)
                .HasColumnName("user_id");

            builder.Property(e => e.RoleId)
                .HasColumnName("role_id");
        }
    }
}
