using System.Text.Json;

namespace Virtuoso.Interface
{
    public interface IGraph
    {
        JsonDocument GetAllGraphs();

        JsonDocument CreateGraph(string graphName);
    }
}
