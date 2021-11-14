using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    public class DomainConfiguration : IEntityTypeConfiguration<API.Domain.Entities.Domain>
    {
        public void Configure(EntityTypeBuilder<API.Domain.Entities.Domain> builder)
        {
            builder.ToTable("domains", "project");

            builder.HasKey(e => e.Id)
                    .HasName("pk_domain");

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.Name)
                .HasColumnName("name");

            builder.Property(e => e.Description)
                .HasColumnName("description");

            builder.Property(e => e.SubjectId)
                .HasColumnName("subject_id");

            builder.Property(e => e.DateCreated)
                .HasColumnName("date_created");
        }
    }
}
