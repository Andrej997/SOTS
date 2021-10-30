using API.Application.Tests.Commands.CreateTest;
using API.Application.Tests.Queries.GetSubjects;
using API.Application.Tests.Queries.GetTests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.WebApi.Controllers
{
    public class TestsController : ApiControllerBase
    {
        [HttpGet]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<List<TestsInfoDto>>> GetTestsInfo(long userId)
        {
            try
            {
                return await Mediator.Send(new GetTestsInfoQuery { UserId = userId });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("subjects")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<List<SubjectDto>>> GetSubjects()
        {
            try
            {
                return await Mediator.Send(new GetSubjectsQuery());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("create")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> CreateTest(CreateTestCommand command)
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
