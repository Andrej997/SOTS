namespace API.Domain.Entities
{
    public class Grade
    {
        public long Id { get; set; }

        public double FromProcentage { get; set; }

        public double ToProcentage { get; set; }

        public string Label { get; set; }
    }
}
