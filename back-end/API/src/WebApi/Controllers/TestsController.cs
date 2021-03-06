using API.Application.Tests.Commands.CreateSubject;
using API.Application.Tests.Commands.CreateTest;
using API.Application.Tests.Commands.DeleteTest;
using API.Application.Tests.Commands.EditSubject;
using API.Application.Tests.Commands.PublishTest;
using API.Application.Tests.Commands.SortBy;
using API.Application.Tests.Commands.UpdateTest;
using API.Application.Tests.Queries.ExportQTI;
using API.Application.Tests.Queries.GetSubjects;
using API.Application.Tests.Queries.GetTakeTest;
using API.Application.Tests.Queries.GetTests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [Route("test")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<TestsInfoDto>> GetTestInfo(long testId)
        {
            try
            {
                return (await Mediator.Send(new GetTestsInfoQuery { TestIds = new List<long> { testId } })).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("export/qti/{testId}")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> ExportQTI(long testId)
        {
            try
            {
                var bytes = await Mediator.Send(new ExportQTIQuery { TestId = testId });
                return Ok(File(bytes, "application/zip", "file.zip"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("publish/{testId}")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> PublishTest(long testId)
        {
            try
            {
                await Mediator.Send(new PublishTestCommand { TestId = testId });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("take-test/{testId}")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<TakeTestDto>> GetTakeTest(long testId)
        {
            try
            {
                return await Mediator.Send(new GetTakeTestQuery { TestId = testId });
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
        [Route("create/subject")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> CreateSubject(CreateSubjectCommand command)
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

        [HttpPut]
        [Route("edit/subject")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> EditSubjectCommand(EditSubjectCommand command)
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
        [Route("create")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<long>> CreateTest(CreateTestCommand command)
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

        [HttpPut]
        [Route("sortby")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> SortBy(SortByCommand command)
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

        [HttpPut]
        [Route("update")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> UpdateTest(UpdateTestCommand command)
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
        [Route("detele/{testId}")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> DeleteTest(long testId)
        {
            try
            {
                await Mediator.Send(new DeleteTestCommand { TestId = testId });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
