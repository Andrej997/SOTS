using API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Tests.Commands.DeleteTest
{
    public class DeleteTestCommand : IRequest
    {
        public long TestId { get; set; }
    }

    public class DeleteTestCommandHandler : IRequestHandler<DeleteTestCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public DeleteTestCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(DeleteTestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var test = _context.Tests.Where(test => test.Id == request.TestId).FirstOrDefault();
                if (test != null)
                {
                    _context.Tests.Remove(test);
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
