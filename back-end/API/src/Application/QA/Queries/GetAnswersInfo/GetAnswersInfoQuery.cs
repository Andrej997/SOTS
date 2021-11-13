using API.Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.QA.Queries.GetAnswersInfo
{
    public class GetAnswersInfoQuery : IRequest<List<AnswersInfoDto>>
    {
        public long TestId { get; set; }

        public long QuestionId { get; set; }
    }
    public class GetAnswersInfoQueryHandler : IRequestHandler<GetAnswersInfoQuery, List<AnswersInfoDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly IMapper _mapper;

        public GetAnswersInfoQueryHandler(IApplicationDbContext context,
            IMapper mapper,
            IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
            _mapper = mapper;
        }

        public async Task<List<AnswersInfoDto>> Handle(GetAnswersInfoQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var answerQuery = _context.Answers
                    .Where(answer => answer.QuestionId == request.QuestionId && _context.Questions.Any(question => question.TestId == request.TestId))
                    .Select(answer => new AnswersInfoDto
                    {
                        Id = answer.Id,
                        Text = answer.TextAnswer,
                        IsCorrect = answer.IsCorrect,
                        QuestionId = answer.QuestionId,
                        QuestionText = _context.Questions.Where(question => question.Id == answer.QuestionId).Select(question => question.TextQuestion).FirstOrDefault(),
                        TestId = _context.Questions.Where(question => question.Id == answer.QuestionId).Select(question => question.TestId).FirstOrDefault()
                    });

                return answerQuery.ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
