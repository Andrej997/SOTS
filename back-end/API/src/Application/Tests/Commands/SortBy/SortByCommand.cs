using API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Tests.Commands.SortBy
{
    public class SortByCommand : IRequest
    {
        public long TestId { get; set; }

        public int SortBy { get; set; }
    }

    public class UpdateTestCommandHandler : IRequestHandler<SortByCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public UpdateTestCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(SortByCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var test = _context.Tests
                    .Where(test => test.Id == request.TestId)
                    .FirstOrDefault();

                if (test != null)
                {
                    test.SortBy = request.SortBy;
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
