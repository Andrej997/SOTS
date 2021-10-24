namespace API.Domain.Entities
{
    public class QuestionCompleted
    {
        public long Id { get; set; }

        public long StudentTestsId { get; set; }

        public long QuestionId { get; set; }

        public double CompletedPercentage { get; set; }
    }
}
