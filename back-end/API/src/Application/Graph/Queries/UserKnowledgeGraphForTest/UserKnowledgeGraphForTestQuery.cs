using API.Application.Common.Interfaces;
using API.Application.Common.Models;
using API.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Graph.Queries.UserKnowledgeGraphForTest
{
    public class UserKnowledgeGraphForTestQuery : IRequest<Tuple<List<NodeDto>, List<Edge>>>
    {
        public long UserId { get; set; }

        public long TestId { get; set; }
    }
    public class UserKnowledgeGraphForTestQueryHandler : IRequestHandler<UserKnowledgeGraphForTestQuery, Tuple<List<NodeDto>, List<Edge>>>
    {
        private readonly IApplicationDbContext _context;

        public UserKnowledgeGraphForTestQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<Tuple<List<NodeDto>, List<Edge>>> Handle(UserKnowledgeGraphForTestQuery request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                var studentTest = _context.StudentTests
                    .Where(st => st.Id == request.TestId)
                    .FirstOrDefault();

                var chosenAnswers = _context.ChoosenAnswers
                    .Where(ca => ca.StudentTestId == request.TestId)
                    .ToList();
                var chosenAnswersIds = chosenAnswers.Select(ca => ca.AnswerId).ToList();

                var nodes = _context.Nodes
                    .Where(node => _context.Tests.Any(test => test.Id == studentTest.TestId && node.DomainId == test.DomainId))
                    .Select(node => new NodeDto
                    {
                        Id = node.Id,
                        Label = node.Label,
                        CustomColor = "#837B7F"
                    })
                    .ToList();

                var edges = _context.Edges
                    .Where(edge => _context.Tests.Any(test => test.Id == studentTest.TestId && edge.DomainId == test.DomainId))
                    .ToList();

                var questionEdges = _context.Questions
                    .Where(question => question.TestId == studentTest.TestId)
                    .Select(questionEdge => new
                    {
                        Label = "hasQuestion",
                        Id = "qustionNodeEdge_" + questionEdge.Id,
                        TargetId = "questionNode" + questionEdge.Id,
                        SourceId = questionEdge.ProblemNodeId,
                        NodeLabel = questionEdge.TextQuestion
                    })
                    .ToList();

                foreach (var questionEdge in questionEdges)
                {
                    var greenCounter = 0;
                    var redCounter = 0;
                    var qId = long.Parse(questionEdge.Id.Split('_')[1]);
                    var chosenAnswer = chosenAnswers.Where(ca => ca.QuestionId == qId).ToList();
                    if (chosenAnswer == null)
                    {
                        nodes.Add(new NodeDto
                        {
                            Id = questionEdge.TargetId,
                            Label = questionEdge.NodeLabel,
                            CustomColor = "#ff0000" // red
                        });
                        ++redCounter;
                    }
                    else
                    {
                        var answers = _context.Answers
                            .Where(a => a.QuestionId == qId)
                            .ToList();
                        var allCorrectAnswersCount = answers.Where(a => a.IsCorrect == true).Count();
                        //var allIncorrectAnswersCount = answers.Where(a => a.IsCorrect == false).Count();
                        var chosenAnswersDb = _context.Answers
                            .Where(a => chosenAnswersIds.Contains(a.Id) && a.QuestionId == qId)
                            .ToList();
                        var correctAnswersCount = chosenAnswersDb.Where(a => a.IsCorrect == true).Count();
                        //var incorrectAnswersCount = chosenAnswersDb.Where(a => a.IsCorrect == false).Count();
                        var correctness = ((double)correctAnswersCount / (double)allCorrectAnswersCount) * 100.00;
                        if (correctness > 50)
                        {
                            nodes.Add(new NodeDto
                            {
                                Id = questionEdge.TargetId,
                                Label = questionEdge.NodeLabel,
                                CustomColor = "#42EC61" // green
                            });
                            ++greenCounter;
                        }
                        else
                        {
                            nodes.Add(new NodeDto
                            {
                                Id = questionEdge.TargetId,
                                Label = questionEdge.NodeLabel,
                                CustomColor = "#ff0000" // red
                            });
                            ++redCounter;
                        }
                    }

                    edges.Add(new Edge
                    {
                        Id = questionEdge.Id,
                        Label = questionEdge.Label,
                        SourceId = questionEdge.SourceId,
                        TargetId = questionEdge.TargetId
                    });
                    if (greenCounter > redCounter)
                        nodes.Where(x => x.Id == questionEdge.SourceId).FirstOrDefault().CustomColor = "#42EC61";
                    else
                        nodes.Where(x => x.Id == questionEdge.SourceId).FirstOrDefault().CustomColor = "#ff0000";
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
