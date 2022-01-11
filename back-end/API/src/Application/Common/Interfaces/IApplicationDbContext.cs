using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<UserRole> UserRoles { get; set; }
        DbSet<UserSubject> UserSubjects { get; set; }
        DbSet<StudentTest> StudentTests { get; set; }
        DbSet<Test> Tests { get; set; }
        DbSet<TestTime> TestTimes { get; set; }
        DbSet<Subject> Subjects { get; set; }
        DbSet<Grade> Grades { get; set; }
        DbSet<Question> Questions { get; set; }
        DbSet<QuestionTime> QuestionTimes { get; set; }
        DbSet<Answer> Answers { get; set; }
        DbSet<ChoosenAnswer> ChoosenAnswers { get; set; }
        DbSet<Node> Nodes { get; set; }
        DbSet<Edge> Edges { get; set; }
        DbSet<EdgeRK> EdgeRKs { get; set; }
        DbSet<API.Domain.Entities.Domain> Domains { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
