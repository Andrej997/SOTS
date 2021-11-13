using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.WebApi.Controllers
{
    public class VirtuosoController : ApiControllerBase
    {
        [HttpGet]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> GetQustionsInfo()
        {
            try
            {
                var client = new RestClient($"http://localhost:8890/sparql");
                client.AddDefaultParameter("format", "application/json");
                //client.AddDefaultParameter("format", "text/turtle");
                client.AddDefaultParameter("query", "select distinct ?Concept where {[] a ?Concept} LIMIT 100");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                IRestResponse response = client.Execute(request);
                var content = response.Content;
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
