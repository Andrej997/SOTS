namespace API.Domain.Entities
{
    public class StudentTests
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public long TestId { get; set; }

        public bool TookTest { get; set; }

        public double Points { get; set; }

        public long GradeId { get; set; }
    }
}
