using API.Application.Common.Interfaces;
using API.Application.Common.Models;
using API.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Graph.Queries.ExpectedKnowlageGraphForTest
{
    public class ExpectedKnowlageGraphForTestQuery : IRequest<Tuple<List<NodeDto>, List<Edge>>>
    {
        public long TestId { get; set; }
    }
    public class ExpectedKnowlageGraphForTestQueryHandler : IRequestHandler<ExpectedKnowlageGraphForTestQuery, Tuple<List<NodeDto>, List<Edge>>>
    {
        private readonly IApplicationDbContext _context;

        public ExpectedKnowlageGraphForTestQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<Tuple<List<NodeDto>, List<Edge>>> Handle(ExpectedKnowlageGraphForTestQuery request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                var nodes = _context.Nodes
                    .Where(node => _context.Tests.Any(test => test.Id == request.TestId && node.DomainId == test.DomainId))
                    .Select(node => new NodeDto
                    {
                        Id = node.Id,
                        Label = node.Label,
                        CustomColor = "#837B7F"
                    })
                    .ToList();

                var edges = _context.Edges
                    .Where(edge => _context.Tests.Any(test => test.Id == request.TestId && edge.DomainId == test.DomainId))
                    .ToList();

                var questionEdges = _context.Questions
                    .Where(question => question.TestId == request.TestId)
                    .Select(questionEdge => new 
                    {
                        Label = "hasQuestion",
                        Id = "qustionNodeEdge" + questionEdge.Id,
                        TargetId = "questionNode" + questionEdge.Id,
                        SourceId = questionEdge.ProblemNodeId,
                        NodeLabel = questionEdge.TextQuestion
                    })
                    .ToList();

                foreach (var questionEdge in questionEdges)
                {
                    nodes.Add(new NodeDto
                    {
                        Id = questionEdge.TargetId,
                        Label = questionEdge.NodeLabel,
                        CustomColor = "#42EC61"
                    });
                    edges.Add(new Edge
                    {
                        Id = questionEdge.Id,
                        Label = questionEdge.Label,
                        SourceId = questionEdge.SourceId,
                        TargetId = questionEdge.TargetId
                    });
                    nodes.Where(x => x.Id == questionEdge.SourceId).FirstOrDefault().CustomColor = "#42B1EC";
                }

                return new Tuple<List<NodeDto>, List<Edge>>(nodes, edges);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
