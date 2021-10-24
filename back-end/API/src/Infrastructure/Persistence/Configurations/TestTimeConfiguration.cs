using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    public class TestTimeConfiguration : IEntityTypeConfiguration<TestTime>
    {
        public void Configure(EntityTypeBuilder<TestTime> builder)
        {
            builder.ToTable("test_time", "project");

            builder.HasKey(e => e.Id)
                    .HasName("pk_test_time");

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.Start)
                .HasColumnName("start");

            builder.Property(e => e.End)
                .HasColumnName("end");
        }
    }
}
