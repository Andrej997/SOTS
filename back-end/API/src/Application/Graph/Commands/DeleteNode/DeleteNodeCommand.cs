using API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Graph.Commands.DeleteNode
{
    public class DeleteNodeCommand : IRequest
    {
        public string NodeId { get; set; }
    }
    public class DeleteNodeCommandHandler : IRequestHandler<DeleteNodeCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public DeleteNodeCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(DeleteNodeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var node = _context.Nodes
                        .Where(node => node.Id == request.NodeId)
                        .FirstOrDefault();

                if (node != null)
                {
                    _context.Nodes.Remove(node);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                return Unit.Value;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
