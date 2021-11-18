using API.Application.Common.Interfaces;
using API.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.QA.Commands.CreateQuestion
{
    public class CreateQuestionCommand : IRequest
    {
        public long TestId { get; set; }

        public string QuestionText { get; set; }

        public long Points { get; set; }
    }
    public class CreateAnswerCommandHandler : IRequestHandler<CreateQuestionCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public CreateAnswerCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (_context.Questions.Any(question => question.TestId == request.TestId))
                {
                    _context.Questions
                        .Add(new Question
                        {
                            TestId = request.TestId,
                            TextQuestion = request.QuestionText,
                            Points = request.Points
                        });

                    await _context.SaveChangesAsync(cancellationToken);
                }

                return Unit.Value;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
