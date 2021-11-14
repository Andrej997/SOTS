using API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Virtuoso.Interface;

namespace API.Application.Virtuoso.Queries.GetAllTriple
{
    public class GetAllTripleQuerry : IRequest<JsonDocument>
    {
        public long Limit { get; set; }
    }

    public class GetAllTripleQuerryHandler : IRequestHandler<GetAllTripleQuerry, JsonDocument>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly ITriple _vTriple;

        public GetAllTripleQuerryHandler(IApplicationDbContext context,
            ITriple vTriple,
            IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
            _vTriple = vTriple;
        }

        public async Task<JsonDocument> Handle(GetAllTripleQuerry request, CancellationToken cancellationToken)
        {
            try
            {
                return _vTriple.GetAllTriples(request.Limit);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
