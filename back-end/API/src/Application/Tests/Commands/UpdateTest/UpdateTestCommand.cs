using API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Tests.Commands.UpdateTest
{
    public class UpdateTestCommand : IRequest
    {
        public long TestId { get; set; }

        public string TestText { get; set; }

        public long MaxPoints { get; set; }
    }

    public class UpdateTestCommandHandler : IRequestHandler<UpdateTestCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public UpdateTestCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(UpdateTestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var test = _context.Tests
                    .Where(test => test.Id == request.TestId)
                    .FirstOrDefault();

                if (test != null)
                {
                    test.Name = request.TestText;
                    test.MaxPoints = request.MaxPoints;
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
