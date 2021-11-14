using Microsoft.Extensions.Options;
using RestSharp;
using System.Text.Json;
using Virtuoso.Interface;

namespace Virtuoso.Service
{
    public class Triple : ITriple
    {
        private readonly Model.Virtuoso config;

        public Triple(IOptions<Model.Virtuoso> settings)
        {
            config = settings.Value;
        }

        public JsonDocument GetAllTriples(long limit)
        {
            var client = new RestClient(config.Url);
            client.AddDefaultParameter("format", config.Format);
            client.AddDefaultParameter("query", $"SELECT ?subject ?predicate ?object WHERE {{ ?subject ?predicate ?object }} LIMIT {limit}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return JsonDocument.Parse(response.Content);
        }

        public JsonDocument GetAllTriplesFromGraph(string graph, long limit)
        {
            var client = new RestClient(config.Url);
            client.AddDefaultParameter("format", config.Format);
            client.AddDefaultParameter("query", $"SELECT * FROM NAMED {graph} {{ GRAPH ?graph {{ ?subject ?predicate ?object }} }} LIMIT {limit}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return JsonDocument.Parse(response.Content);
        }

        // graph : <http://www.example.com/ont#books>
        // subject : <ex:s1>
        // predicate : <ex:p1>
        // object : <ex:o1>
        public JsonDocument InsertTriple(Model.Triple triple)
        {
            var client = new RestClient(config.Url);
            client.AddDefaultParameter("format", config.Format);
            client.AddDefaultParameter("query", $"INSERT DATA {{ GRAPH {triple.Graph} {{ {triple.Subject} {triple.Predicate} {triple.Object} }} }}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return JsonDocument.Parse(response.Content);
        }
    }
}
