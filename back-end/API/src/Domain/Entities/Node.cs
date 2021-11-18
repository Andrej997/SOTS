using System;
using System.Text.Json;

namespace API.Domain.Entities
{
    public class Node
    {
        public string Id { get; set; }

        public string Label { get; set; }

        public long DomainId { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
