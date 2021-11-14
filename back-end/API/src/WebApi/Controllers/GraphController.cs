﻿using API.Application.Graph.Commands.CreateEdge;
using API.Application.Graph.Commands.CreateNode;
using API.Application.Graph.Commands.DeleteEdge;
using API.Application.Graph.Commands.DeleteNode;
using API.Application.Graph.Queries.GetEdges;
using API.Application.Graph.Queries.GetNodes;
using API.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.WebApi.Controllers
{
    public class GraphController : ApiControllerBase
    {
        [HttpPost]
        [Route("get/nodes")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<List<Node>>> GetNodes(GetNodesQuerry querry)
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
        [Route("get/edges")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<List<Edge>>> GetEdges(GetEdgesQuerry querry)
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
        [Route("create/node")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> CreateNode(CreateNodeCommand command)
        {
            try
            {
                await Mediator.Send(command);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("create/edge")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> CreateEdge(CreateEdgeCommand command)
        {
            try
            {
                await Mediator.Send(command);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("node/{nodeId}")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> DeleteNode(string nodeId)
        {
            try
            {
                await Mediator.Send(new DeleteNodeCommand { NodeId = nodeId });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("delete/edges")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> DeleteEdge(DeleteEdgeCommand command)
        {
            try
            {
                await Mediator.Send(command);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}