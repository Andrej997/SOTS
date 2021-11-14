using API.Application.Common.Interfaces;
using API.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly IDateTime _dateTime;

        public GetNodesQuerryHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<List<Node>> Handle(GetNodesQuerry request, CancellationToken cancellationToken)
        {
            try
            {
                IQueryable<Node> nodesQuerry = _context.Nodes;

                if (request.DomainId > 0)
                    nodesQuerry = nodesQuerry
                        .Where(node => _context.DomainNodes.Any(dn => dn.NodeId == node.Id && dn.DomainId == request.DomainId));

                return nodesQuerry.ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
