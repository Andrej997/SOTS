using API.Application.Virtuoso.Commands.CreateGraph;
using API.Application.Virtuoso.Commands.CreateTriple;
using API.Application.Virtuoso.Queries.GetAllGraphs;
using API.Application.Virtuoso.Queries.GetAllTriple;
using API.Application.Virtuoso.Queries.GetTriplesFromGraph;
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

        [HttpGet]
        [Route("get/all/triples/limit/{limit}")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<JsonDocument>> GetAllTriple(long limit)
        {
            try
            {
                return await Mediator.Send(new GetAllTripleQuerry { Limit = limit });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("get/graph/triples")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<JsonDocument>> GetTriplesFromGraph(GetTriplesFromGraphQuerry querry)
        {
            try
            {
                return await Mediator.Send(querry);
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

        [HttpPost]
        [Route("create/triple")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<JsonDocument>> CreateTriple(CreateTripleCommand command)
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
