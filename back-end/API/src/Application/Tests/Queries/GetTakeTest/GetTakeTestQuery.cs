using API.Application.Common.Interfaces;
using API.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
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

        public GetTestsInfoQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<TakeTestDto> Handle(GetTakeTestQuery request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                var domainId = _context.Domains
                    .Where(domain => _context.Tests.Any(test => test.Id == request.TestId && test.DomainId == domain.Id))
                    .Select(domain => domain.Id)
                    .FirstOrDefault();

                //var markedNodes = _context.Nodes
                //    .Where(node => node.DomainId == domainId)
                //    .Select(node => new MarkedNode
                //    {
                //        Id = node.Id,
                //        PermanentMark = false,
                //        TemporaryMark = false
                //    })
                //    .ToList();

                var nodes = _context.Nodes
                    .Where(node => node.DomainId == domainId)
                    .ToList();

                var edges = _context.Edges
                    .Where(edge => edge.DomainId == domainId)
                    .ToList();

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
                

                //var sortedNodes = DFS(markedNodes, edges).Select(x => x.Id).ToList();

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

        private List<Node> Kahn(List<Node> nodes, List<Edge> edges)
        {
            var sorted = new List<Node>();

            while(nodes.Count > 0)
            {
                var n = nodes.First();
                nodes.Remove(n);
                sorted.Add(n);

                foreach (var m in nodes)
                {
                    if (edges.Any(edge => n.Id == edge.SourceId && m.Id == edge.TargetId))
                    {
                        var e = edges.Where(edge => n.Id == edge.SourceId && m.Id == edge.TargetId).FirstOrDefault();
                        edges.Remove(e);
                        if (!edges.Any(edge => edge.TargetId == m.Id))
                        {
                            nodes.Remove(m);
                            sorted.Add(m);
                            break;
                        }
                           
                    }
                }
            }

            return sorted;
        }

        private List<Node> DFS(List<MarkedNode> markedNodes, List<Edge> edges)
        {
            var sorted = new List<Node>();

            MarkedNode n = null;
            while ((n = markedNodes.Where(mn => mn.PermanentMark == false).FirstOrDefault()) != null) 
            {
                Visit(n, markedNodes, edges, sorted);
            }

            return sorted;
        }

        private void Visit(MarkedNode n, List<MarkedNode> markedNodes, List<Edge> edges, List<Node> sorted)
        {
            if (n.PermanentMark == true)
                return;

            if (n.TemporaryMark == true)
                return;

            n.TemporaryMark = true;

            foreach (var m in markedNodes)
            {
                if (edges.Any(edge => n.Id == edge.SourceId && m.Id == edge.TargetId))
                    Visit(m, markedNodes, edges, sorted);
            }

            n.TemporaryMark = false;
            n.PermanentMark = true;
            sorted.Add(n.node);
        }
    }

    public class MarkedNode : Node
    {
        public bool TemporaryMark{ get; set; }

        public bool PermanentMark { get; set; }

        public Node node
        {
            get
            {
                return (Node)this;
            }
        }
    }
}
