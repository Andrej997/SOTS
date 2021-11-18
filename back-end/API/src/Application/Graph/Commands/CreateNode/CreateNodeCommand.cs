using API.Application.Common.Interfaces;
using API.Domain.Entities;
using MediatR;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Graph.Commands.CreateNode
{
    public class CreateNodeCommand : IRequest
    {
        public long DomainId { get; set; }

        public string NodeJson { get; set; }
    }
    public class CreateNodeCommandHandler : IRequestHandler<CreateNodeCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public CreateNodeCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(CreateNodeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                JsonDocument json = JsonDocument.Parse(request.NodeJson);
                var node = _context.Nodes
                    .Add(new Node
                    {
                        Id = json.RootElement.GetProperty("id").ToString(),
                        Label = json.RootElement.GetProperty("label").ToString(),
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
