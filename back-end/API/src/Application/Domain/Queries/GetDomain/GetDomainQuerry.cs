using API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Domain.Queries.GetDomain
{
    public class GetDomainQuerry : IRequest<List<DomainDto>>
    {
    }
    public class GetDomainQuerryHandler : IRequestHandler<GetDomainQuerry, List<DomainDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public GetDomainQuerryHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<List<DomainDto>> Handle(GetDomainQuerry request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                return _context.Domains
                    .Select(domain => new DomainDto
                    {
                        Id = domain.Id,
                        Name = domain.Name,
                        Description = domain.Description,
                        SubjectId = domain.SubjectId,
                        SubjectName = _context.Subjects.Where(subject => subject.Id == domain.SubjectId).Select(subject => subject.Name).FirstOrDefault()
                    })
                    .ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
