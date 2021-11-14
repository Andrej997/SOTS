using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    public class DomainNodesConfiguration : IEntityTypeConfiguration<DomainNodes>
    {
        public void Configure(EntityTypeBuilder<DomainNodes> builder)
        {
            builder.ToTable("domain_nodes", "project");

            builder.HasKey(e => e.Id)
                    .HasName("pk_domain_nodes");

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.DomainId)
                .HasColumnName("domain_id");

            builder.Property(e => e.NodeId)
                .HasColumnName("node_id");
        }
    }
}
