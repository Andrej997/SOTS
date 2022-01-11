using API.Application.Common.Interfaces;
using API.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Tests.Queries.GetTests
{
    public class GetTestsInfoQuery : IRequest<List<TestsInfoDto>>
    {
        public long UserId { get; set; }

        public List<long> TestIds { get; set; }
    }

    public class GetTestsInfoQueryHandler : IRequestHandler<GetTestsInfoQuery, List<TestsInfoDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public GetTestsInfoQueryHandler(IApplicationDbContext context,
            IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<List<TestsInfoDto>> Handle(GetTestsInfoQuery request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                var testsQuery = _context.Tests
                    .Select(test => new TestsInfoDto
                    {
                        Created = test.CreatedAt,
                        CreatorId = test.CreatorId,
                        CreatorName = _context.Users
                            .Where(user => user.Id == test.CreatorId)
                            .Select(user => user.Name + " " + user.Surname)
                            .FirstOrDefault(),
                        Name = test.Name,
                        Id = test.Id,
                        MaxPoints = test.MaxPoints,
                        QuestionCount = test.Questions.Count,
                        SubjectName = _context.Subjects
                            .Where(subject => subject.Id == test.SubjectId)
                            .Select(subject => subject.Name)
                            .FirstOrDefault(),
                        SubjectId = test.SubjectId,
                        Start = _context.TestTimes
                            .Where(tt => tt.Id == test.TestTimeId)
                            .Select(tt => tt.Start)
                            .FirstOrDefault(),
                        End = _context.TestTimes
                            .Where(tt => tt.Id == test.TestTimeId)
                            .Select(tt => tt.End)
                            .FirstOrDefault(),
                        Published = test.Published,
                        DomainId = test.DomainId,
                        DomainName = _context.Domains
                            .Where(domain => domain.Id == test.DomainId)
                            .Select(domain => domain.Name)
                            .FirstOrDefault(),
                        SortBy = test.SortBy
                    });

                var userRoleId = _context.UserRoles.Where(ur => ur.UserId == request.UserId).Select(ur => ur.RoleId).FirstOrDefault();

                if (userRoleId == (long)Roles.proffesor)
                    testsQuery = testsQuery
                        .Where(test => _context.UserSubjects.Any(us => us.UserId == test.CreatorId && us.SubjectId == test.SubjectId));
                else if (userRoleId == (long)Roles.student)
                    testsQuery = testsQuery
                        .Where(test => _context.UserSubjects.Any(us => us.UserId == request.UserId && us.SubjectId == test.SubjectId) 
                            && test.Published == true
                            && !_context.StudentTests.Any(st => st.UserId == request.UserId && st.TestId == test.Id)
                            );

                if (request.TestIds != null && request.TestIds.Any())
                    testsQuery = testsQuery
                        .Where(test => request.TestIds.Contains(test.Id));

                return testsQuery.OrderBy(test => test.Name).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
