using API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Virtuoso.Interface;

namespace API.Application.Virtuoso.Queries.GetTriplesFromGraph
{
    public class GetTriplesFromGraphQuerry : IRequest<JsonDocument>
    {
        public string Graph { get; set; }

        public long Limit { get; set; }
    }

    public class GetTriplesFromGraphQuerryHandler : IRequestHandler<GetTriplesFromGraphQuerry, JsonDocument>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly ITriple _vTriple;

        public GetTriplesFromGraphQuerryHandler(IApplicationDbContext context,
            ITriple vTriple,
            IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
            _vTriple = vTriple;
        }

        public async Task<JsonDocument> Handle(GetTriplesFromGraphQuerry request, CancellationToken cancellationToken)
        {
            try
            {
                return _vTriple.GetAllTriplesFromGraph(request.Graph, request.Limit);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
