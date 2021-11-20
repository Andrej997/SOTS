using API.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Users.Queries.GetStudentTests
{
    public class GetStudentTestsQuery : IRequest<object>
    {
        public long UserId { get; set; }
    }
    public class GetTestsInfoQueryHandler : IRequestHandler<GetStudentTestsQuery, object>
    {
        private readonly IApplicationDbContext _context;

        public GetTestsInfoQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<object> Handle(GetStudentTestsQuery request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                var studentTest = _context.StudentTests
                    .Include(st => st.Test)
                    .Where(st => st.UserId == request.UserId)
                    .Select(st => new
                    {
                        Id = st.Id,
                        GradeLabel = _context.Grades.Where(g => g.Id == st.GradeId).Select(g => g.Label).FirstOrDefault(),
                        Points = st.Points,
                        MaxTestPoints = st.Test.MaxPoints,
                        StudentTestStarted = st.TestStarted,
                        StudentTestFinished = st.TestFinished,
                        TestStarted = _context.TestTimes.Where(tt => tt.Id == st.Test.TestTimeId).Select(tt => tt.Start).FirstOrDefault(),
                        TestEnded = _context.TestTimes.Where(tt => tt.Id == st.Test.TestTimeId).Select(tt => tt.End).FirstOrDefault(),
                        TestId = st.TestId,
                        TestName = st.Test.Name,
                        SubjectLabel = _context.Subjects.Where(s => s.Id == st.Test.SubjectId).Select(d => d.Name).FirstOrDefault(),
                        DomainLabel = _context.Domains.Where(d => d.Id == st.Test.DomainId).Select(d => d.Name).FirstOrDefault(),
                        Questions = _context.QuestionTimes
                            .Where(qt => qt.StudentTestsId == st.Id)
                            .Select(qt => new
                            {
                                Id = qt.QuestionId,
                                QuestionStart = qt.QuestionStart,
                                QuestionEnd = qt.QuestionEnd,
                                QuestionText = _context.Questions
                                    .Where(q => q.Id == qt.QuestionId)
                                    .Select(q => q.TextQuestion)
                                    .FirstOrDefault(),
                                ProblemLabel = _context.Nodes
                                    .Where(node => _context.Questions.Any(question => question.Id == qt.QuestionId && node.Id == question.ProblemNodeId))
                                    .Select(node => node.Label)
                                    .FirstOrDefault(),
                                Answers = _context.Answers
                                    .Where(answer => answer.QuestionId == qt.QuestionId)
                                    .Select(answer => new
                                    {
                                        Id = answer.Id,
                                        TextAnswer = answer.TextAnswer,
                                        IsCorrect = answer.IsCorrect,
                                        DidStudentAnswer = _context.ChoosenAnswers
                                            .Any(ca => ca.QuestionId == qt.QuestionId && ca.StudentTestId == st.Id && ca.AnswerId == answer.Id),
                                        AnswerDated = _context.ChoosenAnswers
                                            .Where(ca => ca.QuestionId == qt.QuestionId && ca.StudentTestId == st.Id && ca.AnswerId == answer.Id)
                                            .Select(ca => ca.AnswerDated)
                                            .FirstOrDefault(),
                                    })
                                    .ToList()
                            })
                            .ToList()
                    })
                    .ToList();

                return studentTest;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
