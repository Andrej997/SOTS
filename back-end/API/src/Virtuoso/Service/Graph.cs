using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Text.Json;
using Virtuoso.Interface;

namespace Virtuoso.Service
{
    public class Graph : IGraph
    {
        private readonly Model.Virtuoso config;

        public Graph(IOptions<Model.Virtuoso> settings)
        {
            config = settings.Value;
        }

        public JsonDocument CreateGraph(string graphName)
        {
            var client = new RestClient(config.Url);
            client.AddDefaultParameter("format", config.Format);
            client.AddDefaultParameter("query", $"CREATE GRAPH <http://www.example.com/ont#{graphName}>");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            IRestResponse response = client.Execute(request);
            return JsonDocument.Parse(response.Content);
        }

        public JsonDocument GetAllGraphs()
        {
            var client = new RestClient(config.Url);
            client.AddDefaultParameter("format", config.Format);
            client.AddDefaultParameter("query", "SELECT DISTINCT ?graph WHERE { GRAPH ?graph { ?subject ?predicate ?object }}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return JsonDocument.Parse(response.Content);
        }
    }
}
