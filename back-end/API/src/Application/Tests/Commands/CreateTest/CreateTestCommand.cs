using API.Application.Common.Interfaces;
using API.Domain.Entities;
using API.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Tests.Commands.CreateTest
{
    public class CreateTestCommand : IRequest
    {
        public string Name { get; set; }

        public long SubjectId { get; set; }

        public List<Question> Questions { get; set; }

        public long CreatorId { get; set; }

        public long MaxPoints { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
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

                if (_context.UserRoles.Any(ur => ur.UserId == request.CreatorId && (ur.RoleId != (long)Roles.admin && ur.RoleId != (long)Roles.proffesor)))
                    throw new Exception("Not allowd to create a test!");

                if (!_context.Subjects.Any(ur => ur.Id == request.SubjectId))
                    throw new Exception("Subject not found!");

                var testTimeDb = _context.TestTimes.Add(new TestTime
                {
                    Start = request.Start,
                    End = request.End
                });

                await _context.SaveChangesAsync(cancellationToken);

                var testDb = _context.Tests
                   .Add(new Test
                   {
                       Name = request.Name,
                       SubjectId = request.SubjectId,
                       CreatedAt = _dateTime.UtcNow,
                       CreatorId = request.CreatorId,
                       MaxPoints = request.MaxPoints,
                       Questions = request.Questions,
                       TestTimeId = testTimeDb.Entity.Id
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
