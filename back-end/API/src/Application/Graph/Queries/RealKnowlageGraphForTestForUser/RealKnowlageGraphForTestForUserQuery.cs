using API.Application.Common.Interfaces;
using API.Application.Common.Models;
using API.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Graph.Queries.RealKnowlageGraphForTestForUser
{
    public class RealKnowlageGraphForTestForUserQuery : IRequest<Tuple<List<NodeDto>, List<Edge>>>
    {
        public long StundetTestId { get; set; }

        public long UserId { get; set; }
    }
    public class RealKnowlageGraphForTestForUserQueryHandler : IRequestHandler<RealKnowlageGraphForTestForUserQuery, Tuple<List<NodeDto>, List<Edge>>>
    {
        private readonly IApplicationDbContext _context;

        public RealKnowlageGraphForTestForUserQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<Tuple<List<NodeDto>, List<Edge>>> Handle(RealKnowlageGraphForTestForUserQuery request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                var testId = _context.StudentTests.Where(st => st.Id == request.StundetTestId && st.UserId == request.UserId).Select(st => st.TestId).FirstOrDefault();
                var nodes = _context.Nodes
                    .Where(node => _context.Tests.Any(test => test.Id == testId && node.DomainId == test.DomainId))
                    .Select(node => new NodeDto
                    {
                        Id = node.Id,
                        Label = node.Label,
                        CustomColor = "#837B7F"
                    })
                    .ToList();

                var edges = _context.Edges
                    .Where(edge => _context.Tests.Any(test => test.Id == testId && edge.DomainId == test.DomainId))
                    .ToList();

                var questionEdges = _context.Questions
                    .Where(question => question.TestId == testId)
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
                    var questionId = long.Parse(questionEdge.TargetId.Split("questionNode")[1]);
                    if (IsQuestionAnsweredTrue(request.StundetTestId, questionId))
                    {
                        nodes.Add(new NodeDto
                        {
                            Id = questionEdge.TargetId,
                            Label = questionEdge.NodeLabel,
                            CustomColor = "#42EC61"
                        });
                        if (nodes.Where(x => x.Id == questionEdge.SourceId).FirstOrDefault().CustomColor != "#D53424")
                            nodes.Where(x => x.Id == questionEdge.SourceId).FirstOrDefault().CustomColor = "#42EC61";
                    }
                    else
                    {
                        nodes.Add(new NodeDto
                        {
                            Id = questionEdge.TargetId,
                            Label = questionEdge.NodeLabel,
                            CustomColor = "#D53424"
                        });
                        nodes.Where(x => x.Id == questionEdge.SourceId).FirstOrDefault().CustomColor = "#D53424";
                    }
                    edges.Add(new Edge
                    {
                        Id = questionEdge.Id,
                        Label = questionEdge.Label,
                        SourceId = questionEdge.SourceId,
                        TargetId = questionEdge.TargetId
                    });
                }

                return new Tuple<List<NodeDto>, List<Edge>>(nodes, edges);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool IsQuestionAnsweredTrue(long studentTestId, long questionId)
        {
            var answers = _context.Answers.Where(answer => answer.QuestionId == questionId).ToList();
            foreach (var answer in answers)
            {
                var choosenAnswer = _context.ChoosenAnswers.Where(canswer => canswer.StudentTestId == studentTestId && canswer.AnswerId == answer.Id && canswer.QuestionId == questionId).FirstOrDefault();
                if (choosenAnswer != null && !answer.IsCorrect)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
