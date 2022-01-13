using API.Application.Common.Interfaces;
using API.Application.Users.Commands.EditUser;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest
    {
        public long Id { get; set; }
    }
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteUserCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if (!_context.StudentTests.Any(st => st.UserId == request.Id))
            {
                if (!_context.Tests.Any(st => st.CreatorId == request.Id))
                {
                    var user = _context.Users
                        .Where(u => u.Id == request.Id)
                        .FirstOrDefault();
                    var userRoles = _context.UserRoles.Where(ur => ur.UserId == request.Id).ToList();
                    _context.UserRoles.RemoveRange(userRoles);

                    var userSubjects = _context.UserSubjects.Where(ur => ur.UserId == request.Id).ToList();
                    _context.UserSubjects.RemoveRange(userSubjects);

                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync(cancellationToken);
                    
                }
            }
            else throw new Exception("Can't delete user has tests");

            return Unit.Value;
        }
    }
}
