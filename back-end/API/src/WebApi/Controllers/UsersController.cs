using API.Application.Users.Commands.CreateUser;
using API.Application.Users.Commands.DeleteUser;
using API.Application.Users.Commands.EditUser;
using API.Application.Users.Commands.FinishTest;
using API.Application.Users.Commands.Login;
using API.Application.Users.Commands.StartTest;
using API.Application.Users.Queries.ChoosenAnswers;
using API.Application.Users.Queries.GetRoles;
using API.Application.Users.Queries.GetStudentTests;
using API.Application.Users.Queries.GetUser;
using API.Application.Users.Queries.GetUsers;
using API.Domain.Entities;
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
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            try
            {
                return await Mediator.Send(new GetUsersQuery());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<UserDto>> GetUser(long id)
        {
            try
            {
                return await Mediator.Send(new GetUserQuery { Id = id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("roles")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<List<Role>>> GetRoles()
        {
            try
            {
                return await Mediator.Send(new GetRolesQuery());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

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

        [HttpPut]
        [Route("edit")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> EditUser(EditUserCommand command)
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
        [Route("delete/{id}")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            try
            {
                await Mediator.Send(new DeleteUserCommand { Id = id });
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
