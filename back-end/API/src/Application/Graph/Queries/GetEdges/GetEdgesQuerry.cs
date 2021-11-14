using API.Application.Common.Interfaces;
using API.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Graph.Queries.GetEdges
{
    public class GetEdgesQuerry : IRequest<List<Edge>>
    {
        public long DomainId { get; set; }
    }
    public class GetEdgesQuerryHandler : IRequestHandler<GetEdgesQuerry, List<Edge>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public GetEdgesQuerryHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<List<Edge>> Handle(GetEdgesQuerry request, CancellationToken cancellationToken)
        {
            try
            {
                IQueryable<Edge> edgesQuerry = _context.Edges;

                if (request.DomainId > 0)
                    edgesQuerry = edgesQuerry
                        .Where(edge => edge.DomainId == request.DomainId);

                return edgesQuerry.ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
