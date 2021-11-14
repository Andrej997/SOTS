using API.Application.Virtuoso.Commands.CreateGraph;
using API.Application.Virtuoso.Queries.GetAllGraphs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.WebApi.Controllers
{
    public class VirtuosoController : ApiControllerBase
    {
        [HttpGet]
        [Route("get/all/graphs")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<JsonDocument>> GetAllGraphs()
        {
            try
            {
                return await Mediator.Send(new GetAllGraphsQuerry());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("create/graph")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<JsonDocument>> CreateGraph(CreateGraphCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
