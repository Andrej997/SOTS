using API.Application.QA.Commands.CreateAnswer;
using API.Application.QA.Commands.CreateQuestion;
using API.Application.QA.Commands.CreateQuestionAnswers;
using API.Application.QA.Commands.DeleteAnswer;
using API.Application.QA.Commands.EditAnswer;
using API.Application.QA.Queries.GetAnswersInfo;
using API.Application.QA.Queries.GetQustionsInfo;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.WebApi.Controllers
{
    public class QAController : ApiControllerBase
    {
        [HttpGet]
        [Route("test/{testId}")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<List<QustionsInfoDto>>> GetQustionsInfo(long testId)
        {
            try
            {
                return await Mediator.Send(new GetQustionsInfoQuery { TestId = testId });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("test/{testId}/question/{questionId}")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<List<AnswersInfoDto>>> GetAnswersInfo(long testId, long questionId)
        {
            try
            {
                return await Mediator.Send(new GetAnswersInfoQuery { TestId = testId, QuestionId = questionId });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("create/answer")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> CreateQuestionAnswers(CreateAnswerCommand command)
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
        [Route("create/question")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> CreateQuestion(CreateQuestionCommand command)
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
        public async Task<IActionResult> CreateQuestionAnswers(CreateQuestionAnswersCommand command)
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
        public async Task<IActionResult> EditAnswer(EditAnswerCommand command)
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
        [Route("detele/answer/{answerId}")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<IActionResult> DeleteAnswer(long answerId)
        {
            try
            {
                await Mediator.Send(new DeleteAnswerCommand { AnswerId = answerId });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
