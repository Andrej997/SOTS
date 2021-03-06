using API.Application.Common.Interfaces;
using API.Domain.Entities;
using MediatR;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Graph.Commands.CreateEdge
{
    public class CreateEdgeCommand : IRequest
    {
        public long DomainId { get; set; }

        public string EdgeJson { get; set; }
    }
    public class CreateEdgeCommandHandler : IRequestHandler<CreateEdgeCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public CreateEdgeCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(CreateEdgeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                JsonDocument json = JsonDocument.Parse(request.EdgeJson);
                _context.Edges
                    .Add(new Edge
                    {
                        Id = json.RootElement.GetProperty("id").ToString(),
                        Label = json.RootElement.GetProperty("label").ToString(),
                        SourceId = json.RootElement.GetProperty("source").ToString(),
                        TargetId = json.RootElement.GetProperty("target").ToString(),
                        DomainId = request.DomainId,
                        DateCreated = _dateTime.UtcNow
                    });

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
