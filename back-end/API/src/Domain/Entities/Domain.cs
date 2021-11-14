using System;

namespace API.Domain.Entities
{
    public class Domain
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long SubjectId { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
