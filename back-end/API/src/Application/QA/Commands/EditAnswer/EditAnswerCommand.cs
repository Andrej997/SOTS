using API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.QA.Commands.EditAnswer
{
    public class EditAnswerCommand : IRequest
    {
        public long AnswerId { get; set; }

        public string AnswerText { get; set; }

        public bool IsCorrect { get; set; }
    }

    public class EditAnswerCommandHandler : IRequestHandler<EditAnswerCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public EditAnswerCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(EditAnswerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var answer = _context.Answers
                    .Where(answer => answer.Id == request.AnswerId).FirstOrDefault();

                if (answer != null)
                {
                    answer.TextAnswer = request.AnswerText;
                    answer.IsCorrect = request.IsCorrect;
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
