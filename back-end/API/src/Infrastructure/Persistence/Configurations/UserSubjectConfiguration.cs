using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infrastructure.Persistence.Configurations
{
    public class UserSubjectConfiguration : IEntityTypeConfiguration<UserSubject>
    {
        public void Configure(EntityTypeBuilder<UserSubject> builder)
        {
            builder.ToTable("user_subject", "project");

            builder.HasKey(e => new { e.SubjectId, e.UserId })
                    .HasName("pk_user_subject");

            builder.Property(e => e.SubjectId)
                .HasColumnName("subject_id");

            builder.Property(e => e.UserId)
                .HasColumnName("user_id");
        }
    }
}
