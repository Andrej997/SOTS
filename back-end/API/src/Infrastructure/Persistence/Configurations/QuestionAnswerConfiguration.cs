using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    public class QuestionAnswerConfiguration : IEntityTypeConfiguration<QuestionAnswer>
    {
        public void Configure(EntityTypeBuilder<QuestionAnswer> builder)
        {
            builder.ToTable("questions_answers", "project");

            builder.HasKey(e => new { e.QuestionId, e.AnswerId })
                    .HasName("pk_questions_answers");

            builder.Property(e => e.QuestionId)
                .HasColumnName("question_id");

            builder.Property(e => e.AnswerId)
                .HasColumnName("answer_id");
        }
    }
}
