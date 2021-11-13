using System.Text.Json;

namespace API.Domain.Entities
{
    public class Node
    {
        public long Id { get; set; }

        public JsonDocument NodeJson { get; set; }
    }
}
