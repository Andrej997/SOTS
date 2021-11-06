using API.Application.Common.Interfaces;
using API.Domain.Entities;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace API.Application.QA.Commands.SaveUserAnswer
{
    public class SaveUserAnswerCommand : IRequest
    {
        public long StudentTestId { get; set; }

        public long QuestionId { get; set; }

        public long AnswerId { get; set; }
    }

    public class SaveUserAnswerCommandHandler : IRequestHandler<SaveUserAnswerCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public SaveUserAnswerCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(SaveUserAnswerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!_context.StudentTests.Any(stundetTest => stundetTest.Id == request.StudentTestId))
                    throw new Exception("Wrong exam");

                if (!_context.Questions.Any(question => question.Id == request.QuestionId))
                    throw new Exception("Wrong question");

                if (!_context.Answers.Any(answer => answer.Id == request.AnswerId && answer.QuestionId == request.QuestionId))
                    throw new Exception("Wrong answer");

                _context.ChoosenAnswers
                    .Add(new ChoosenAnswer
                    {
                        AnswerId = request.AnswerId,
                        QuestionId = request.QuestionId,
                        StudentTestId = request.StudentTestId,
                        AnswerDated = _dateTime.UtcNow
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
