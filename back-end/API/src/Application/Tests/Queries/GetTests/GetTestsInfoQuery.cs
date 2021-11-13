using API.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Tests.Queries.GetTests
{
    public class GetTestsInfoQuery : IRequest<List<TestsInfoDto>>
    {
        public long UserId { get; set; }
    }

    public class GetTestsInfoQueryHandler : IRequestHandler<GetTestsInfoQuery, List<TestsInfoDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly IMapper _mapper;

        public GetTestsInfoQueryHandler(IApplicationDbContext context,
            IMapper mapper,
            IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
            _mapper = mapper;
        }

        public async Task<List<TestsInfoDto>> Handle(GetTestsInfoQuery request, CancellationToken cancellationToken)
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
                        Published = test.Published
                    });

                if (request.UserId == (long)Domain.Enums.Roles.proffesor || request.UserId == (long)Domain.Enums.Roles.student)
                    testsQuery = testsQuery
                        .Where(test => _context.UserSubjects.Any(us => us.UserId == test.CreatorId && us.SubjectId == test.SubjectId));

                return testsQuery.OrderBy(test => test.Name).ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
