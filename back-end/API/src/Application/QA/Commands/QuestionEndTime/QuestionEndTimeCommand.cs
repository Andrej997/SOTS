using System;
using API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace API.Application.QA.Commands.QuestionEndTime
{
    public class QuestionEndTimeCommand : IRequest
    {
        public long StudentTestsId { get; set; }
        public long QuestionId { get; set; }
    }

    public class QuestionStartTimeCommandHandler : IRequestHandler<QuestionEndTimeCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public QuestionStartTimeCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(QuestionEndTimeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var questionTime = _context.QuestionTimes
                    .Where(qt => qt.QuestionId == request.QuestionId && qt.StudentTestsId == request.StudentTestsId)
                    .FirstOrDefault();
                questionTime.QuestionEnd = _dateTime.UtcNow;

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
