﻿using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("questions", "project");

            builder.HasKey(e => e.Id)
                    .HasName("pk_question");

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.TextQuestion)
                .HasColumnName("text_question");

            builder.Property(e => e.CreatedAt)
                .HasColumnName("created_at");
        }
    }
}