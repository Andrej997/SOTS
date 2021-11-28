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

                var nodes = _context.Nodes
                    .Where(node => node.DomainId == domainId)
                    .Select(node => new MarkedNode
                    {
                        Id = node.Id,
                        PermanentMark = false,
                        TemporaryMark = false
                    })
                    .ToList();

                var edges = _context.Edges
                    .Where(edge => edge.DomainId == domainId)
                    .ToList();

                var sortedNodes = DFS(nodes, edges).Select(x => x.Id).ToList();

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

                for (int i = sortedNodes.Count - 1; i >= 0; i--)
                {
                    var nodeId = sortedNodes[i];
                    var questionsInProblem = questions.Where(q => q.ProblemNodeId == nodeId).ToList();
                    sortedQuestions.AddRange(questionsInProblem);
                }

                testsQuery.Questions = sortedQuestions;

                return testsQuery;
            }
            catch (Exception)
            {
                throw;
            }
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
