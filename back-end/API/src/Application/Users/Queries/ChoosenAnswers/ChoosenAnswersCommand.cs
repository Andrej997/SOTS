using API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Users.Queries.ChoosenAnswers
{
    public class ChoosenAnswersCommand : IRequest<ChoosenAnswersDto>
    {
        public long StudentTestId { get; set; }

        public long UserId { get; set; }
    }
    public class ChoosenAnswersCommandHandler : IRequestHandler<ChoosenAnswersCommand, ChoosenAnswersDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public ChoosenAnswersCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<ChoosenAnswersDto> Handle(ChoosenAnswersCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var retVal = new ChoosenAnswersDto();

                var studentTest = _context.StudentTests
                    .Where(st => st.Id == request.StudentTestId && st.UserId == request.UserId)
                    .FirstOrDefault();

                retVal.Grade.Grade = _context.Grades
                    .Where(grade => grade.Id == studentTest.GradeId)
                    .Select(grade => grade.Label)
                    .FirstOrDefault();
                retVal.Grade.Points = studentTest.Points;

                var test = _context.Tests
                    .Where(test => test.Id == studentTest.TestId)
                    .FirstOrDefault();

                retVal.TestText = test.Name;

                var choosenAnswers = _context.ChoosenAnswers
                    .Where(ca => ca.StudentTestId == request.StudentTestId)
                    .ToList();

                var questions = _context.Questions
                    .Where(question => question.TestId == test.Id)
                    .ToList();

                foreach (var question in questions)
                {
                    var questionDto = new QuestionDto
                    {
                        QustionText = question.TextQuestion
                    };
                    var answers = _context.Answers
                        .Where(answer => answer.QuestionId == question.Id)
                        .ToList();
                    foreach (var answer in answers)
                    {
                        var answerDto = new AnswerDto
                        {
                            AnswerText = answer.TextAnswer,
                            IsCorrect = answer.IsCorrect,
                            IsChoosen = choosenAnswers.Any(ca => ca.AnswerId == answer.Id && ca.QuestionId == question.Id && ca.StudentTestId == studentTest.Id),
                        };
                        questionDto.Awnsers.Add(answerDto);
                    }
                    retVal.Questions.Add(questionDto);
                }

                return retVal;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
