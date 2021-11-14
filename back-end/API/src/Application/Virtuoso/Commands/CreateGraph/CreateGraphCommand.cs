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

namespace API.Application.Virtuoso.Commands.CreateGraph
{
    public class CreateGraphCommand : IRequest<JsonDocument>
    {
        public string GraphName { get; set; }
    }

    public class CreateGraphCommandHandler : IRequestHandler<CreateGraphCommand, JsonDocument>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly IGraph _vGraph;

        public CreateGraphCommandHandler(IApplicationDbContext context,
            IGraph vGraph,
            IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
            _vGraph = vGraph;
        }

        public async Task<JsonDocument> Handle(CreateGraphCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return _vGraph.CreateGraph(request.GraphName);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
