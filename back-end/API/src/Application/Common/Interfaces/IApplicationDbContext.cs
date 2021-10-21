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
        DbSet<Test> Tests { get; set; }
        DbSet<Subject> Subjects { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
