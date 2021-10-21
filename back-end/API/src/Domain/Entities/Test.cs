using System;

namespace API.Domain.Entities
{
    public class Test
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long SubjectId { get; set; }

        public long CreatorId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
