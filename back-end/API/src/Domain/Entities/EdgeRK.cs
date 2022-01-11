using System;

namespace API.Domain.Entities
{
    public class EdgeRK
    {
        public long Id { get; set; }

        public long TestId { get; set; }

        public string SourceId { get; set; }

        public string TargetId { get; set; }
    }
}
