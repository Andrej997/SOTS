using API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Domain.Commands.CreateDomain
{
    public class CreateDomainCommand : IRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public long SubjectId { get; set; }
    }
    public class CreateNodeCommandHandler : IRequestHandler<CreateDomainCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public CreateNodeCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(CreateDomainCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _context.Domains
                    .Add(new API.Domain.Entities.Domain
                    {
                        DateCreated = _dateTime.UtcNow,
                        Description = request.Description,
                        Name = request.Name,
                        SubjectId = request.SubjectId
                    });

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
