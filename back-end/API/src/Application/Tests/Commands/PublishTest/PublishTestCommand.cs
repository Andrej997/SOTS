using API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Tests.Commands.PublishTest
{
    public class PublishTestCommand : IRequest
    {
        public long TestId { get; set; }
    }

    public class PublishTestCommandHandler : IRequestHandler<PublishTestCommand>
    {
        private readonly IApplicationDbContext _context;

        public PublishTestCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(PublishTestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var test = _context.Tests
                    .Where(test => test.Id == request.TestId)
                    .FirstOrDefault();

                if (test != null)
                {
                    if (!_context.Questions.Any(question => question.TestId == test.Id))
                        throw new Exception("Can't publish, test does not have questions");

                    if (_context.Answers.Any(answer => !_context.Questions.Any(question => question.TestId == test.Id && answer.QuestionId == question.Id)))
                        throw new Exception("Can't publish, some questions does not have answers");

                    test.Published = true;
                    await _context.SaveChangesAsync(cancellationToken);
                }
                else throw new Exception("Unknown id");

                return Unit.Value;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
