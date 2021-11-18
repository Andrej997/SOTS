using API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Graph.Commands.DeleteEdge
{
    public class DeleteEdgeCommand : IRequest
    {
        public List<string> EdgeIds { get; set; }
    }
    public class DeleteEdgeCommandHandler : IRequestHandler<DeleteEdgeCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public DeleteEdgeCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(DeleteEdgeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                foreach (var edgeId in request.EdgeIds)
                {
                    var edge = _context.Edges
                        .Where(edge => edge.Id == edgeId)
                        .FirstOrDefault();
                    if (edge != null)
                        _context.Edges.Remove(edge);
                }
                

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
