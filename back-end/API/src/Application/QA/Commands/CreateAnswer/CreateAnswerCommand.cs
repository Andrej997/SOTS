using API.Application.Common.Interfaces;
using API.Domain.Entities;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.QA.Commands.CreateAnswer
{
    public class CreateAnswerCommand : IRequest
    {
        public long TestId { get; set; }

        public long QuestionId { get; set; }

        public string AnswerText { get; set; }

        public bool IsCorrect { get; set; }
    }

    public class CreateAnswerCommandHandler : IRequestHandler<CreateAnswerCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public CreateAnswerCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(CreateAnswerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (_context.Questions.Any(question => question.TestId == request.TestId && question.Id == request.QuestionId))
                {
                    _context.Answers
                        .Add(new Answer
                        {
                            QuestionId = request.QuestionId,
                            TextAnswer = request.AnswerText,
                            IsCorrect = request.IsCorrect
                        });

                    await _context.SaveChangesAsync(cancellationToken);
                }

                return Unit.Value;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
