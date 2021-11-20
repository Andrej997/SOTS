namespace API.Application.QA.Queries.GetQustionsInfo
{
    public class QustionsInfoDto
    {
        public long Id { get; set; }

        public long TestId { get; set; }

        public string Text { get; set; }

        public long AnswersCount { get; set; }

        public long Points { get; set; }

        public string ProblemNodeId { get; set; }

        public string ProblemNodeLabel { get; set; }
    }
}
