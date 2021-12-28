using API.Application.Common.Interfaces;
using API.Application.Common.Models;
using API.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

                var client = new RestClient("http://192.168.0.20:5003");
                var restRequest = new RestRequest("api/calculate/kst", Method.POST);
                restRequest.AddJsonBody(dynamic);
                var response = client.Execute(restRequest);

                return null;// new Tuple<List<NodeDto>, List<Edge>>(nodes, edges);
            }
            catch (Exception)
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
