using API.Application.Users.Commands.CreateUser;
using API.Application.Users.Commands.FinishTest;
using API.Application.Users.Commands.Login;
using API.Application.Users.Commands.StartTest;
using API.Application.Users.Queries.ChoosenAnswers;
using API.Application.Users.Queries.GetStudentTests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.WebApi.Controllers
{
    public class UsersController : ApiControllerBase
    {
        [HttpGet]
        [Route("tests")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<object>> GetStudentTests(long userId)
        {
            try
            {
                return await Mediator.Send(new GetStudentTestsQuery { UserId = userId });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("start/test")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<long>> StartTest(StartTestCommand command)
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
        [Route("finish/test")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<object>> FinishTest(FinishTestCommand command)
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
        [Route("choosenanswers")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<ChoosenAnswersDto>> ChoosenAnswers(ChoosenAnswersCommand command)
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
        [Route("create")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> CreateUser(CreateUserCommand command)
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
        [Route("login")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<UserDto>> Login(LoginCommand command)
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
