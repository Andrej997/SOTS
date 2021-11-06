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
    public class StudentTestConfiguration : IEntityTypeConfiguration<StudentTest>
    {
        public void Configure(EntityTypeBuilder<StudentTest> builder)
        {
            builder.ToTable("student_tests", "project");

            builder.HasKey(e => e.Id)
                    .HasName("pk_student_test");

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.TestId)
                .IsRequired()
                .HasColumnName("test_id");

            builder.Property(e => e.UserId)
                .IsRequired()
                .HasColumnName("user_id");

            builder.Property(e => e.Points)
                .HasDefaultValue(0)
                .HasColumnName("points");

            builder.Property(e => e.GradeId)
                .HasColumnName("grade_id");

            builder.Property(e => e.TestStarted)
                .IsRequired()
                .HasColumnName("test_started");

            builder.Property(e => e.TestFinished)
                .HasColumnName("test_finished");
        }
    }
}
