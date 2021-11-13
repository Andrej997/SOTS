using API.Application.Common.Interfaces;
using API.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Graph.Queries.GetEdges
{
    public class GetEdgesQuerry : IRequest<List<Edge>>
    {
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
                var edges = _context.Edges
                    .ToList();

                return edges;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
