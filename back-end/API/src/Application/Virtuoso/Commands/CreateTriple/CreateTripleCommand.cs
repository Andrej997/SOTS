using API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Virtuoso.Interface;
using Virtuoso.Model;

namespace API.Application.Virtuoso.Commands.CreateTriple
{
    public class CreateTripleCommand : IRequest<JsonDocument>
    {
        public string Graph { get; set; }

        public string Subject { get; set; }

        public string Predicate { get; set; }

        public string Object { get; set; }
    }

    public class CreateTripleCommandHandler : IRequestHandler<CreateTripleCommand, JsonDocument>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly ITriple _vTriple;

        public CreateTripleCommandHandler(IApplicationDbContext context,
            ITriple vTriple,
            IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
            _vTriple = vTriple;
        }

        public async Task<JsonDocument> Handle(CreateTripleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return _vTriple.InsertTriple(new Triple
                {
                    Graph = request.Graph,
                    Subject = request.Subject,
                    Predicate = request.Predicate,
                    Object = request.Object
                });
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
