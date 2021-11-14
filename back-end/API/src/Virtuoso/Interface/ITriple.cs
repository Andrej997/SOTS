using System.Text.Json;

namespace Virtuoso.Interface
{
    public interface ITriple
    {
        JsonDocument GetAllTriples(long limit);

        JsonDocument GetAllTriplesFromGraph(string graph, long limit);

        JsonDocument InsertTriple(Model.Triple triple);
    }
}
