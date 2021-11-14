using System.Text.Json;

namespace API.Domain.Entities
{
    public class Edge
    {
        public long Id { get; set; }

        public JsonDocument EdgeJson { get; set; }

        public long DomainId { get; set; }
    }
}
