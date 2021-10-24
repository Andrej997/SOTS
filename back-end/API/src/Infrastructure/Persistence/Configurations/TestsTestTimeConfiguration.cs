using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    public class TestsTestTimeConfiguration : IEntityTypeConfiguration<TestsTestTime>
    {
        public void Configure(EntityTypeBuilder<TestsTestTime> builder)
        {
            builder.ToTable("tests_test_time", "project");

            builder.HasKey(e => new { e.TestId, e.TestTimeId })
                    .HasName("pk_tests_test_time");

            builder.Property(e => e.TestId)
                .HasColumnName("test_id");

            builder.Property(e => e.TestTimeId)
                .HasColumnName("test_time_id");
        }
    }
}
