using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    public class TestConfiguration : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.ToTable("tests", "project");

            builder.HasKey(e => e.Id)
                    .HasName("pk_test");

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.Name)
                .HasColumnName("name");

            builder.Property(e => e.SubjectId)
                .HasColumnName("subject_id");

            builder.Property(e => e.CreatorId)
                .HasColumnName("creator_id");

            builder.Property(e => e.CreatedAt)
                .HasColumnName("created_at");

            builder.Property(e => e.TestTimeId)
                .HasColumnName("test_time_id");

            builder.Property(e => e.MaxPoints)
                .HasColumnName("max_points");
        }
    }
}
