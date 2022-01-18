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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Tests.Queries.GetTakeTest
{
    public class GetTakeTestQuery : IRequest<TakeTestDto>
    {
        public long TestId { get; set; }
    }

    public class GetTestsInfoQueryHandler : IRequestHandler<GetTakeTestQuery, TakeTestDto>
    {
        private readonly IApplicationDbContext _context;
        private string KstApi;

        public GetTestsInfoQueryHandler(IApplicationDbContext context, IOptions<Kst> settings)
        {
            _context = context;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<TakeTestDto> Handle(GetTakeTestQuery request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                var sortBy = _context.Tests
                    .Where(test => test.Id == request.TestId)
                    .Select(test => test.SortBy)
                    .FirstOrDefault();

                var domainId = _context.Domains
                    .Where(domain => _context.Tests.Any(test => test.Id == request.TestId && test.DomainId == domain.Id))
                    .Select(domain => domain.Id)
                    .FirstOrDefault();

                var nodes = _context.Nodes
                    .Where(node => node.DomainId == domainId)
                    .ToList();

                var edges = _context.Edges
                    .Where(edge => edge.DomainId == domainId)
                    .ToList();


                var sortedNodes = new List<string>();
                if (sortBy == 0)
                    sortedNodes = SortByExpexted(nodes, edges);
                else
                    sortedNodes = SortByReal(request.TestId);

                var testsQuery = _context.Tests
                    .Where(test => test.Id == request.TestId)
                    .Select(test => new TakeTestDto
                    {
                        CreatorName = _context.Users
                            .Where(user => user.Id == test.CreatorId)
                            .Select(user => user.Name + " " + user.Surname)
                            .FirstOrDefault(),
                        Name = test.Name,
                        Id = test.Id,
                        MaxPoints = test.MaxPoints,
                        SubjectName = _context.Subjects
                            .Where(subject => subject.Id == test.SubjectId)
                            .Select(subject => subject.Name)
                            .FirstOrDefault(),
                        Start = _context.TestTimes
                            .Where(tt => tt.Id == test.TestTimeId)
                            .Select(tt => tt.Start)
                            .FirstOrDefault(),
                        End = _context.TestTimes
                            .Where(tt => tt.Id == test.TestTimeId)
                            .Select(tt => tt.End)
                            .FirstOrDefault(),
                        Questions = _context.Questions
                            .Where(question => question.TestId == test.Id)
                            .Select(question => new TakeTestQuestionDto
                            {
                                Id = question.Id,
                                Question = question.TextQuestion,
                                ProblemNodeId = question.ProblemNodeId,
                                Answers = _context.Answers
                                    .Where(answer => answer.QuestionId == question.Id)
                                    .Select(answer => new TakeTestQuestionAnswerDto
                                    {
                                        Id = answer.Id,
                                        Answer = answer.TextAnswer
                                    })
                                    .ToList()
                            })
                            .ToList()
                    }).FirstOrDefault();

                var questions = testsQuery.Questions;
                var sortedQuestions = new List<TakeTestQuestionDto>();

                for (int i = 0; i < sortedNodes.Count; i++)
                {
                    var nodeId = sortedNodes[i];
                    var questionsInProblem = questions.Where(q => q.ProblemNodeId == nodeId).ToList();
                    foreach (var qip in questionsInProblem)
                    {
                        if (!sortedQuestions.Contains(qip))
                            sortedQuestions.Add(qip);
                    }
                }

                testsQuery.Questions = sortedQuestions;

                return testsQuery;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GetNextLayer(Dictionary<int, List<Node>> layers, List<Node> nodes, List<Edge> edges)
        {
            List<Node> currenLayer = null;
            if (layers.Count == 1) // if only lonely nodes
            {
                currenLayer = new List<Node>();
                for (var i = nodes.Count - 1; i >= 0; i--)
                {
                    var firstLayerEdges = edges.Where(edge => nodes[i].Id == edge.TargetId).Count();
                    if (firstLayerEdges == 0)
                    {
                        currenLayer.Add(nodes[i]);
                        nodes.RemoveAt(i);
                    }
                }

                layers.Add(layers.Count, currenLayer);
            }
            else
            {
                currenLayer = new List<Node>();
                var prevLayer = layers.Where(x => x.Key == layers.Count - 1).Select(x => x.Value).FirstOrDefault();
                foreach (var node in prevLayer)
                {
                    var nodeEdges = edges.Where(edge => edge.SourceId == node.Id).Select(edge => edge.TargetId).ToList();
                    var nodesFromNodeEdges = nodes.Where(n => nodeEdges.Contains(n.Id)).ToList();
                    currenLayer.AddRange(nodesFromNodeEdges);
                    nodesFromNodeEdges.ForEach(n => nodes.Remove(n));
                }
                layers.Add(layers.Count, currenLayer);
            }
            if (nodes.Any())
                GetNextLayer(layers, nodes, edges);
        }

        private List<string> SortLayers(List<Node> preLayer, List<Node> currentLayer, List<Edge> edges)
        {
            var sortedArray = new List<string>();

            foreach (var node in preLayer)
            {
                var targets = edges.Where(edge => edge.SourceId == node.Id).Select(edge => edge.TargetId).ToList();
                foreach (var nodeId in targets)
                {
                    if (!sortedArray.Contains(nodeId))
                        sortedArray.Add(nodeId);
                }
            }

            currentLayer = currentLayer.OrderBy(o => sortedArray.IndexOf(o.Id)).ToList();

            return sortedArray;
        }

        private List<string> SortByExpexted(List<Node> nodes, List<Edge> edges)
        {
            var lonelyNodes = nodes.Where(node => !edges.Any(edge => edge.SourceId == node.Id) && !edges.Any(edge => edge.TargetId == node.Id)).ToList();
            lonelyNodes.ForEach(node => nodes.Remove(node));
            var layers = new Dictionary<int, List<Node>>();
            layers.Add(0, lonelyNodes);

            GetNextLayer(layers, nodes, edges);
            var sortedNodes = new List<string>();
            sortedNodes.AddRange(layers[0].Select(n => n.Id).ToList());
            if (layers.Count > 1)
            {
                sortedNodes.AddRange(layers[1].Select(n => n.Id).ToList());
                for (int i = 2; i < layers.Count; i++)
                {
                    sortedNodes.AddRange(SortLayers(layers[i - 1], layers[i], edges));
                }
            }
            return sortedNodes;
        }

        private List<string> SortByReal(long testId)
        {
            var dynamic = new ExpandoObject() as IDictionary<string, Object>;

            var test = _context.Tests
                .Include(test => test.Questions)
                    .ThenInclude(questions => questions.Answers)
                .Where(test => test.Id == testId)
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
            }

            var nodes = new List<Node>();
            foreach (var node in domainNodes)
            {
                nodes.Add(new Node
                {
                    Id = node.Id,
                    Label = node.Label,
                    DomainId = node.DomainId
                });
            }

            var lonelyNodes = nodes.Where(node => !edges.Any(edge => edge.SourceId == node.Id) && !edges.Any(edge => edge.TargetId == node.Id)).ToList();
            lonelyNodes.ForEach(node => nodes.Remove(node));
            var layers = new Dictionary<int, List<Node>>();
            layers.Add(0, lonelyNodes);

            GetNextLayer(layers, nodes, edges);
            var sortedNodes = new List<string>();
            sortedNodes.AddRange(layers[0].Select(n => n.Id).ToList());
            if (layers.Count > 1)
            {
                sortedNodes.AddRange(layers[1].Select(n => n.Id).ToList());
                for (int i = 2; i < layers.Count; i++)
                {
                    sortedNodes.AddRange(SortLayers(layers[i - 1], layers[i], edges));
                }
            }
            return sortedNodes;
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
