using API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Virtuoso.Interface;

namespace API.Application.Virtuoso.Queries.GetAllGraphs
{
    public class GetAllGraphsQuerry : IRequest<JsonDocument>
    {
    }

    public class GetAllGraphsQuerryHandler : IRequestHandler<GetAllGraphsQuerry, JsonDocument>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly IGraph _vGraph;

        public GetAllGraphsQuerryHandler(IApplicationDbContext context,
            IGraph vGraph,
            IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
            _vGraph = vGraph;
        }

        public async Task<JsonDocument> Handle(GetAllGraphsQuerry request, CancellationToken cancellationToken)
        {
            try
            {
                return _vGraph.GetAllGraphs();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }

}
