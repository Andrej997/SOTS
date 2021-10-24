using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    public class StudentTestsConfiguration : IEntityTypeConfiguration<StudentTests>
    {
        public void Configure(EntityTypeBuilder<StudentTests> builder)
        {
            builder.ToTable("student_tests", "project");

            builder.HasKey(e => e.Id)
                    .HasName("pk_student_test");

            builder.Property(e => e.UserId)
                .HasColumnName("user_id");

            builder.Property(e => e.TestId)
                .HasColumnName("test_id");

            builder.Property(e => e.TookTest)
                .HasColumnName("took_test");

            builder.Property(e => e.Points)
                .HasColumnName("points");

            builder.Property(e => e.GradeId)
                .HasColumnName("grade_id");
        }
    }
}
