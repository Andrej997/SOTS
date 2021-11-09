using API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.QA.Commands.QuestionStartTime
{
    public class QuestionStartTimeCommand : IRequest
    {
        public long StudentTestsId { get; set; }
        public long QuestionId { get; set; }
    }

    public class QuestionStartTimeCommandHandler : IRequestHandler<QuestionStartTimeCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public QuestionStartTimeCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(QuestionStartTimeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _context.QuestionTimes
                    .Add(new Domain.Entities.QuestionTime
                    {
                        QuestionId = request.QuestionId,
                        StudentTestsId = request.StudentTestsId,
                        QuestionStart = _dateTime.UtcNow
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
