using API.Application.Common.Interfaces;
using API.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Graph.Queries.GetNodes
{
    public class GetNodesQuerry : IRequest<List<Node>>
    {
        public long DomainId { get; set; }
    }
    public class GetNodesQuerryHandler : IRequestHandler<GetNodesQuerry, List<Node>>
    {
        private readonly IApplicationDbContext _context;

        public GetNodesQuerryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<List<Node>> Handle(GetNodesQuerry request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                IQueryable<Node> nodesQuerry = _context.Nodes;

                if (request.DomainId > 0)
                    nodesQuerry = nodesQuerry
                        .Where(node => node.DomainId == request.DomainId);

                return nodesQuerry.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
