using System;
using System.Text.Json;

namespace API.Domain.Entities
{
    public class Edge
    {
        public string Id { get; set; }

        public string Label { get; set; }

        public string SourdeId { get; set; }

        public string TargetId { get; set; }

        public long DomainId { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
