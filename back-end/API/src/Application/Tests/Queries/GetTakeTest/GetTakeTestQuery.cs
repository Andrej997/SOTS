using API.Application.Common.Interfaces;
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

        public async Task<TakeTestDto> Handle(GetTakeTestQuery request, CancellationToken cancellationToken)
        {
            try
            {
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
                    });

                return testsQuery.FirstOrDefault();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
