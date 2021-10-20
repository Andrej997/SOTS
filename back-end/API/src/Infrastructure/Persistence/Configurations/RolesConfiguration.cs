using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    public class RolesConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("roles", "project");

            builder.HasKey(e => e.Id)
                    .HasName("pk_role");

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.Name)
                .HasColumnName("name");
        }
    }
}
