using API.Application.Domain.Commands.CreateDomain;
using API.Application.Domain.Queries.GetDomain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.WebApi.Controllers
{
    public class DomainController : ApiControllerBase
    {
        [HttpGet]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<List<DomainDto>>> GetDomain()
        {
            try
            {
                return await Mediator.Send(new GetDomainQuerry());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("create/domain")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> CreateDomain(CreateDomainCommand command)
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
