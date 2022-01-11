using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Infrastructure.Persistence.Configurations
{
    public class EdgeRKConfiguration : IEntityTypeConfiguration<EdgeRK>
    {
        public void Configure(EntityTypeBuilder<EdgeRK> builder)
        {
            builder.ToTable("edges_rk", "project");

            builder.HasKey(e => e.Id)
                    .HasName("pk_edge");

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.TestId)
                .HasColumnName("test_id");

            builder.Property(e => e.SourceId)
                .HasColumnName("souce_id");

            builder.Property(e => e.TargetId)
                .HasColumnName("target_id");
        }
    }
}
