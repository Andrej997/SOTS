using API.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.QA.Queries.GetQustionsInfo
{
    public class GetQustionsInfoQuery : IRequest<List<QustionsInfoDto>>
    {
        public long TestId { get; set; }
    }
    public class GetQustionsInfoQueryHandler : IRequestHandler<GetQustionsInfoQuery, List<QustionsInfoDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly IMapper _mapper;

        public GetQustionsInfoQueryHandler(IApplicationDbContext context,
            IMapper mapper,
            IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
            _mapper = mapper;
        }

        public async Task<List<QustionsInfoDto>> Handle(GetQustionsInfoQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var questionsQuery = _context.Questions
                    .Where(question => question.TestId == request.TestId)
                    .Select(question => new QustionsInfoDto
                    {
                        Id = question.Id,
                        TestId = question.TestId,
                        Points = question.Points,
                        Text = question.TextQuestion,
                        AnswersCount = question.Answers.Count
                    });

                return questionsQuery.ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
