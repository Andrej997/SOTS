using API.Application.Common.Interfaces;
using API.Domain.Entities;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Users.Commands.StartTest
{
    public class StartTestCommand : IRequest<long>
    {
        public long TestId { get; set; }

        public long UserId { get; set; }
    }

    public class StartTestCommandHandler : IRequestHandler<StartTestCommand, long>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public StartTestCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<long> Handle(StartTestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!_context.Tests.Any(test => test.Id == request.TestId))
                    throw new Exception("Test not found");

                if (!_context.Users.Any(user => user.Id == request.UserId))
                    throw new Exception("User not found");

                var startTest = _context.StudentTests
                    .Add(new StudentTest
                    {
                        TestId = request.TestId,
                        UserId = request.UserId,
                        TestStarted = _dateTime.UtcNow,
                        GradeId = 1
                    });

                await _context.SaveChangesAsync(cancellationToken);

                return startTest.Entity.Id;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
