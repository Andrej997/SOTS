using API.Application.Common.Interfaces;
using API.Application.Users.Commands.Login;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Users.Queries.GetUser
{
    public class GetUserQuery : IRequest<UserDto>
    {
        public long Id { get; set; }
    }

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IApplicationDbContext _context;

        public GetUserQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return _context.Users
                .Where(user => user.Id == request.Id)
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Name = user.Name,
                    RoleId = _context.UserRoles.Where(ur => ur.UserId == user.Id).Select(ur => ur.RoleId).FirstOrDefault(),
                    Role = _context.Roles.Where(r => _context.UserRoles.Any(ur => ur.UserId == user.Id && ur.RoleId == r.Id)).Select(r => r.Name).FirstOrDefault(),
                    Surname = user.Surname,
                    Subjects = _context.Subjects.Where(s => _context.UserSubjects.Any(us => us.UserId == user.Id && s.Id == us.SubjectId)).ToList(),
                    SubjectIds = _context.Subjects.Where(s => _context.UserSubjects.Any(us => us.UserId == user.Id && s.Id == us.SubjectId)).Select(s => s.Id).ToList()
                })
                .FirstOrDefault();
        }
    }
}
