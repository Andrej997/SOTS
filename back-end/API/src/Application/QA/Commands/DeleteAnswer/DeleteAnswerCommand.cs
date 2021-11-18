using API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.QA.Commands.DeleteAnswer
{
    public class DeleteAnswerCommand : IRequest
    {
        public long AnswerId { get; set; }
    }

    public class DeleteAnswerCommandHandler : IRequestHandler<DeleteAnswerCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public DeleteAnswerCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(DeleteAnswerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var answer = _context.Answers.Where(answer => answer.Id == request.AnswerId).FirstOrDefault();
                if (answer != null)
                {
                    _context.Answers.Remove(answer);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                else throw new Exception("Unknown id");

                return Unit.Value;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
