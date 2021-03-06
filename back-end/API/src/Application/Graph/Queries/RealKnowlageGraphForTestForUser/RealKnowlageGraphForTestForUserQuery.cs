using API.Application.Common.Interfaces;
using API.Application.Common.Models;
using API.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Graph.Queries.RealKnowlageGraphForTestForUser
{
    public class RealKnowlageGraphForTestForUserQuery : IRequest<Tuple<List<NodeDto>, List<Edge>>>
    {
        public long TestId { get; set; }
    }
    public class RealKnowlageGraphForTestForUserQueryHandler : IRequestHandler<RealKnowlageGraphForTestForUserQuery, Tuple<List<NodeDto>, List<Edge>>>
    {
        private readonly IApplicationDbContext _context;
        private string KstApi;

        public RealKnowlageGraphForTestForUserQueryHandler(IApplicationDbContext context, IOptions<Kst> settings)
        {
            _context = context;
            KstApi = settings.Value.API;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<Tuple<List<NodeDto>, List<Edge>>> Handle(RealKnowlageGraphForTestForUserQuery request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                var dynamic = new ExpandoObject() as IDictionary<string, Object>;

                var test = _context.Tests
                    .Include(test => test.Questions)
                        .ThenInclude(questions => questions.Answers)
                    .Where(test => test.Id == request.TestId)
                    .FirstOrDefault();

                var users = _context.Users
                    .Where(user => _context.StudentTests.Any(st => st.TestId == test.Id && user.Id == st.UserId))
                    .ToList();

                var domainNodes = _context.Nodes
                    .Where(node => node.DomainId == test.DomainId)
                    .ToList();

                foreach (var user in users)
                {
                    dynamic.Add(user.Id.ToString(), GetArray(test.Id, user.Id, domainNodes, test.Questions));
                }

                var client = new RestClient(KstApi);
                var restRequest = new RestRequest("api/calculate/kst", Method.POST);
                restRequest.AddJsonBody(dynamic);
                var response = client.Execute(restRequest);


                var jResponse = JObject.Parse(response.Content)["implications"];

                var egdesForReal = _context.EdgeRKs
                    .Where(edge => edge.TestId == request.TestId)
                    .ToList();

                _context.EdgeRKs.RemoveRange(egdesForReal);
                await _context.SaveChangesAsync(cancellationToken);

                var edges = new List<Edge>();
                foreach (var child in jResponse.Children())
                {
                    var from = child.First.Value<int>();
                    var sourceNode = domainNodes[from];
                    var to = child.Last.Value<int>();
                    var targetNode = domainNodes[to];
                    edges.Add(new Edge
                    {
                        Id = sourceNode.Id + "_" + targetNode.Id,
                        SourceId = sourceNode.Id,
                        TargetId = targetNode.Id,
                        Label = "preduslov"
                    });

                    _context.EdgeRKs.Add(new EdgeRK
                    {
                        TestId = request.TestId,
                        SourceId = sourceNode.Id,
                        TargetId = targetNode.Id
                    });
                }
                await _context.SaveChangesAsync(cancellationToken);

                var nodes = new List<NodeDto>();
                foreach (var node in domainNodes)
                {
                    nodes.Add(new NodeDto
                    {
                        Id = node.Id,
                        CustomColor = "#42B1EC",
                        Label = node.Label,
                        DomainId = node.DomainId
                    });
                }

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
                }

                return new Tuple<List<NodeDto>, List<Edge>>(nodes, edges);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private int[] GetArray(long testId, long userId, List<Node> nodes, List<Question> questions)
        {
            var studentTest = _context.StudentTests
                .Where(st => st.UserId == userId && st.TestId == testId)
                .Select(st => st.Id)
                .FirstOrDefault();

            var knownProblems = new int[nodes.Count];

            for (int i = 0; i < nodes.Count - 1; i++)
            {
                var questionsForProblem = questions.Where(q => q.ProblemNodeId == nodes[i].Id).ToList();
                double chosenCorrectAnswers = 0;
                double totalCorrectAnswers = 0;
                foreach (var question in questionsForProblem)
                {
                    var choosenAnswers = _context.ChoosenAnswers
                        .Where(ca => ca.StudentTestId == studentTest && ca.QuestionId == question.Id)
                        .ToList();
                    var correctAnswers = question.Answers.Where(answer => answer.IsCorrect == true).ToList();

                    totalCorrectAnswers += correctAnswers.Count;
                    
                    choosenAnswers.ForEach(choa =>
                    {
                        if (correctAnswers.Any(ca => ca.Id == choa.AnswerId && ca.IsCorrect == true))
                            ++chosenCorrectAnswers;
                    });
                }
                if (chosenCorrectAnswers / totalCorrectAnswers > 0.5)
                    knownProblems[i] = 1;
                else
                    knownProblems[i] = 0;

            }

            return knownProblems;
        }
    }
}
