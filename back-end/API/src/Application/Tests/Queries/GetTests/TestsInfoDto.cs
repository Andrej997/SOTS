using System;

namespace API.Application.Tests.Queries.GetTests
{
    public class TestsInfoDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long QuestionCount { get; set; }

        public long SubjectId { get; set; }

        public long DomainId { get; set; }

        public string SubjectName { get; set; }

        public string DomainName { get; set; }

        public long CreatorId { get; set; }

        public string CreatorName { get; set; }

        public DateTime Created { get; set; }

        public long MaxPoints { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public bool Published { get; set; }

        public int SortBy { get; set; }
    }
}
