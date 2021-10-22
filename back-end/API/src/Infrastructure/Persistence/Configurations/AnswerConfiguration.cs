using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.ToTable("answers", "project");

            builder.HasKey(e => e.Id)
                    .HasName("pk_answer");

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.TextAnswer)
                .HasColumnName("text_answer");
        }
    }
}
