using API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.QA.Commands.DeleteQuestion
{
    public class DeleteQuestionCommand : IRequest
    {
        public long QuestionId { get; set; }
    }
    public class DeleteAnswerCommandHandler : IRequestHandler<DeleteQuestionCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteAnswerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var question = _context.Questions
                    .Where(question => question.Id == request.QuestionId)
                    .FirstOrDefault();

                if (question != null)
                {
                    _context.Questions.Remove(question);
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
