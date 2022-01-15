using API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Tests.Commands.CreateSubject
{
    public class CreateSubjectCommand : IRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
    public class CreateSubjectCommandHandler : IRequestHandler<CreateSubjectCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public CreateSubjectCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(CreateSubjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (_context.Subjects.Any(s => s.Name == request.Name)) throw new Exception("Subject with same name exists!");

                _context.Subjects.Add(new API.Domain.Entities.Subject
                {
                    Name = request.Name,
                    Description = request.Description
                });

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
