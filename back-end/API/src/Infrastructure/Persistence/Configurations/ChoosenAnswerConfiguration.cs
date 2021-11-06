using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    public class ChoosenAnswerConfiguration : IEntityTypeConfiguration<ChoosenAnswer>
    {
        public void Configure(EntityTypeBuilder<ChoosenAnswer> builder)
        {
            builder.ToTable("choosen_answers", "project");

            builder.HasKey(e => new { e.StudentTestId, e.QuestionId, e.AnswerId })
                    .HasName("pk_choosen_answer");

            builder.Property(e => e.StudentTestId)
                .HasColumnName("student_test_id");

            builder.Property(e => e.QuestionId)
                .HasColumnName("question_id");

            builder.Property(e => e.AnswerId)
                .HasColumnName("answer_id");

            builder.Property(e => e.AnswerDated)
                .HasColumnName("answer_dated");
        }
    }
}
