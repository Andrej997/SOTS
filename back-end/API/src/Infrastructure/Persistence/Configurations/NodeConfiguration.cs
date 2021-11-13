﻿using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    public class NodeConfiguration : IEntityTypeConfiguration<Node>
    {
        public void Configure(EntityTypeBuilder<Node> builder)
        {
            builder.ToTable("nodes", "project");

            builder.HasKey(e => e.Id)
                    .HasName("pk_node");

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.NodeJson)
                .HasColumnType("json")
                .HasColumnName("node_json");
        }
    }
}
