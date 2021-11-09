using API.Application.Common.Interfaces;
using API.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Users.Commands.FinishTest
{
    public class FinishTestCommand : IRequest<TestGradeDto>
    {
        public long StudentTestId { get; set; }

        public long TestId { get; set; }

        public long UserId { get; set; }
    }

    public class FinishTestCommandHandler : IRequestHandler<FinishTestCommand, TestGradeDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public FinishTestCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<TestGradeDto> Handle(FinishTestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!_context.StudentTests.Any(studentTest => studentTest.Id == request.StudentTestId
                        && studentTest.TestId == request.TestId
                        && studentTest.UserId == request.UserId))
                    throw new Exception("Wrong test");

                var studentTest = _context.StudentTests
                    .Where(studentTest => studentTest.Id == request.StudentTestId)
                    .FirstOrDefault();

                studentTest.TestFinished = _dateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);

                List<ChoosenAnswer> choosenAnswers = new List<ChoosenAnswer>();

                var questions = _context.Questions
                    .Where(question => question.TestId == studentTest.TestId)
                    .ToList();

                var allCorrectAnswersCount = 0;

                foreach (var question in questions)
                {
                    var answers = _context.Answers
                        .Where(answer => answer.QuestionId == question.Id)
                        .ToList();

                    foreach (var answer in answers)
                    {
                        if (answer.IsCorrect)
                            ++allCorrectAnswersCount;

                        var choosenAnswer = _context.ChoosenAnswers
                            .Where(choosenAnswer => choosenAnswer.StudentTestId == studentTest.Id
                                && choosenAnswer.QuestionId == question.Id
                                && choosenAnswer.AnswerId == answer.Id)
                            .FirstOrDefault();
                        if (choosenAnswer != null)
                            choosenAnswers.Add(choosenAnswer);
                    }
                }

                var correctAnswersCount = 0;

                foreach (var choosenAnswer in choosenAnswers)
                {
                    if (_context.Answers.Where(answer => answer.Id == choosenAnswer.AnswerId && answer.QuestionId == choosenAnswer.QuestionId).Select(answer => answer.IsCorrect).FirstOrDefault())
                        ++correctAnswersCount;
                }

                var persentage = (correctAnswersCount / allCorrectAnswersCount) * 100;
                studentTest.Points = persentage;
                await _context.SaveChangesAsync(cancellationToken);

                var grade = _context.Grades
                        .Where(grade => grade.FromProcentage < persentage && persentage <= grade.ToProcentage)
                        .FirstOrDefault();
                var gradeStr = "F";
                if (grade != null)
                {
                    gradeStr = grade.Label;
                    studentTest.GradeId = grade.Id;
                    await _context.SaveChangesAsync(cancellationToken);
                }

                return new TestGradeDto 
                {
                    Grade = gradeStr,
                    Points = persentage
                };
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
