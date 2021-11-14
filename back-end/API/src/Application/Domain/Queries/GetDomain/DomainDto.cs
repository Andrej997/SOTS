namespace API.Application.Domain.Queries.GetDomain
{
    public class DomainDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long SubjectId { get; set; }

        public string SubjectName { get; set; }
    }
}
