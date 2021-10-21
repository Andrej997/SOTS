using API.Application.Common.Interfaces;
using API.Domain.Entities;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Tests.Commands.CreateTest
{
    public class CreateTestCommand : IRequest
    {
        public string Name { get; set; }

        public long SubjectId { get; set; }

        public long CreatorId { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateTestCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public CreateUserCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(CreateTestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!_context.UserRoles.Any(ur => ur.UserId == request.CreatorId)) 
                    throw new Exception("Creator not found!");

                if (_context.UserRoles.Any(ur => ur.UserId == request.CreatorId && (ur.RoleId != (long)Domain.Enums.Roles.admin && ur.RoleId != (long)Domain.Enums.Roles.proffesor)))
                    throw new Exception("Not allowd to create a test!");

                if (!_context.Subjects.Any(ur => ur.Id == request.SubjectId))
                    throw new Exception("Subject not found!");

                _context.Tests
                   .Add(new Test
                   {
                       Name = request.Name,
                       SubjectId = request.SubjectId,
                       CreatedAt = _dateTime.UtcNow,
                       CreatorId = request.CreatorId
                   });

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
