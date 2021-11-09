using API.Application.Users.Commands.FinishTest;
using System.Collections.Generic;

namespace API.Application.Users.Queries.ChoosenAnswers
{
    public class ChoosenAnswersDto
    {
        public string TestText { get; set; }

        public TestGradeDto Grade { get; set; }

        public List<QuestionDto> Questions { get; set; }

        public ChoosenAnswersDto()
        {
            this.Questions = new List<QuestionDto>();
            Grade = new TestGradeDto();
        }
    }

    public class QuestionDto
    {
        public string QustionText { get; set; }

        public List<AnswerDto> Awnsers { get; set; }

        public QuestionDto()
        {
            this.Awnsers = new List<AnswerDto>();
        }
    }

    public class AnswerDto
    {
        public string AnswerText { get; set; }

        public bool IsChoosen { get; set; }

        public bool IsCorrect { get; set; }
    }
}
