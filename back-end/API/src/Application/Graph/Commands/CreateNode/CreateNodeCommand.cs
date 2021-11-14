﻿using API.Application.Common.Interfaces;
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
                _context.Nodes
                    .Add(new Node
                    {
                        NodeJson = json
                    });

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}