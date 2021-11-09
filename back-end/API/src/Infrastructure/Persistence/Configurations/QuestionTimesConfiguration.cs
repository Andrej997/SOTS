using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    public class QuestionTimesConfiguration : IEntityTypeConfiguration<QuestionTime>
    {
        public void Configure(EntityTypeBuilder<QuestionTime> builder)
        {
            builder.ToTable("question_times", "project");

            builder.HasKey(e => new { e.QuestionStart, e.StudentTestsId })
                    .HasName("pk_question_times");

            builder.Property(e => e.QuestionId)
                .HasColumnName("question_id");

            builder.Property(e => e.StudentTestsId)
                .HasColumnName("student_tests_id");

            builder.Property(e => e.QuestionStart)
                .HasColumnName("question_start");

            builder.Property(e => e.QuestionEnd)
                .HasColumnName("question_end");
        }
    }
}
