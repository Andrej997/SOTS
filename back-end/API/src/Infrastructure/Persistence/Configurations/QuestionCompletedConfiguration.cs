using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    public class QuestionCompletedConfiguration : IEntityTypeConfiguration<QuestionCompleted>
    {
        public void Configure(EntityTypeBuilder<QuestionCompleted> builder)
        {
            builder.ToTable("question_completed", "project");

            builder.HasKey(e => e.Id)
                    .HasName("pk_question_completed");

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.StudentTestsId)
                .HasColumnName("student_tests_id");

            builder.Property(e => e.QuestionId)
                .HasColumnName("question_id");

            builder.Property(e => e.CompletedPercentage)
                .HasColumnName("completed_persentage");
        }
    }
}
