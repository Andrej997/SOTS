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

            builder.Property(e => e.EdgeJson)
                .HasColumnType("json")
                .HasColumnName("edge_json");
        }
    }
}
