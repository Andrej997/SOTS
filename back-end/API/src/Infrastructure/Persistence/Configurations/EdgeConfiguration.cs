using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    public class EdgeConfiguration : IEntityTypeConfiguration<Edge>
    {
        public void Configure(EntityTypeBuilder<Edge> builder)
        {
            builder.ToTable("edges", "project");

            builder.HasKey(e => e.Id)
                    .HasName("pk_edge");

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.Label)
                .HasColumnName("label");

            builder.Property(e => e.SourdeId)
                .HasColumnName("souce_id");

            builder.Property(e => e.TargetId)
                .HasColumnName("target_id");

            builder.Property(e => e.DomainId)
                .HasColumnName("domain_id");

            builder.Property(e => e.DateCreated)
                .HasColumnName("date_created");
        }
    }
}
