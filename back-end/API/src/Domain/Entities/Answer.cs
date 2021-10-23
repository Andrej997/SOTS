namespace API.Domain.Entities
{
    public class Answer
    {
        public long Id { get; set; }

        public string TextAnswer { get; set; }

        public long QuestionId { get; set; }

        public bool IsCorrect { get; set; }
    }
}
