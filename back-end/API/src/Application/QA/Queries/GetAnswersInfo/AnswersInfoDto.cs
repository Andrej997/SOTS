namespace API.Application.QA.Queries.GetAnswersInfo
{
    public class AnswersInfoDto
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public bool IsCorrect { get; set; }

        public long TestId { get; set; }

        public long QuestionId { get; set; }
    }
}
