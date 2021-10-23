using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    public class GradeConfiguration : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.ToTable("grades", "project");

            builder.HasKey(e => e.Id)
                    .HasName("pk_grade");

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.FromProcentage)
                .HasColumnName("from_procentage");

            builder.Property(e => e.ToProcentage)
                .HasColumnName("to_procentage");

            builder.Property(e => e.Label)
                .HasColumnName("label");
        }
    }
}
