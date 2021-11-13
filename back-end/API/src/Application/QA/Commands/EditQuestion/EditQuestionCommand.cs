using API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.QA.Commands.EditQuestion
{
    public class EditQuestionCommand : IRequest
    {
        public long QuestionId { get; set; }

        public string QuestionText { get; set; }

        public long Points { get; set; }
    }
    public class EditQuestionCommandHandler : IRequestHandler<EditQuestionCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public EditQuestionCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(EditQuestionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var question = _context.Questions
                    .Where(question => question.Id == request.QuestionId).FirstOrDefault();

                if (question != null)
                {
                    question.TextQuestion = request.QuestionText;
                    question.Points = request.Points;
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
