using API.Application.Common.Interfaces;
using API.Application.Users.Commands.Login;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Users.Commands.EditUser
{
    public class EditUserCommand : IRequest
    {
        public UserDto User { get; set; }
    }
    public class EditUserCommandHandler : IRequestHandler<EditUserCommand>
    {
        private readonly IApplicationDbContext _context;

        public EditUserCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            var user = _context.Users
                .Where(user => user.Id == request.User.Id)
                .FirstOrDefault();

            if (user != null)
            {
                user.Name = request.User.Name;
                user.Surname = request.User.Surname;
                if (request.User.Password != null && request.User.Password != "")
                    user.PasswordHash = request.User.Password;

                var userRole = _context.UserRoles.Where(ur => ur.UserId == request.User.Id).FirstOrDefault();
                if (userRole.RoleId != request.User.RoleId)
                {
                    _context.UserRoles.Remove(userRole);
                    await _context.SaveChangesAsync(cancellationToken);
                    _context.UserRoles.Add(new API.Domain.Entities.UserRole
                    {
                        UserId = request.User.Id,
                        RoleId = request.User.RoleId
                    });
                    await _context.SaveChangesAsync(cancellationToken);
                }

                var userSubject = _context.UserSubjects.Where(us => us.UserId == request.User.Id).ToList();
                _context.UserSubjects.RemoveRange(userSubject);

                await _context.SaveChangesAsync(cancellationToken);

                foreach (var subject in request.User.SubjectIds)
                {
                    _context.UserSubjects.Add(new API.Domain.Entities.UserSubject
                    {
                        UserId = request.User.Id,
                        SubjectId = subject
                    });
                }

                await _context.SaveChangesAsync(cancellationToken);
            }
            else throw new System.Exception("Unknown user");

            return Unit.Value;
        }
    }
}
