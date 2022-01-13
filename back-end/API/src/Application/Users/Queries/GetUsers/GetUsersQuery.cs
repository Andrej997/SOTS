using API.Application.Common.Interfaces;
using API.Application.Users.Commands.Login;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Users.Queries.GetUsers
{
    public class GetUsersQuery : IRequest<List<UserDto>>
    {
    }

    public class GetTestsInfoQueryHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetTestsInfoQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return _context.Users
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Name = user.Name,
                    RoleId = _context.UserRoles.Where(ur => ur.UserId == user.Id).Select(ur => ur.RoleId).FirstOrDefault(),
                    Role = _context.Roles.Where(r => _context.UserRoles.Any(ur => ur.UserId == user.Id && ur.RoleId == r.Id)).Select(r => r.Name).FirstOrDefault(),
                    Surname = user.Surname,
                    Subjects = _context.Subjects.Where(s => _context.UserSubjects.Any(us => us.UserId == user.Id && s.Id == us.SubjectId)).ToList()
                })
                .ToList();
        }
    }
}
