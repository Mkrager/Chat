using Chat.Application.Contracts;
using Chat.Application.Features.Chat.Commands.PostMessage;
using Chat.Application.Features.Chat.Queries.GetMessageList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController(IMediator mediator, ICurrentUserService currentUserService) : Controller
    {
        [Authorize]
        [HttpGet("{ReceiverUserId}", Name = "GetAllMessages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<MessageListVm>>> GetAllMessages(string ReceiverUserId)
        {
            var dtos = await mediator.Send(new GetMessageListQuery()
            {
                UserId = currentUserService.UserId,
                ReceiverUserId = ReceiverUserId
            });
            return Ok(dtos);
        }

        [Authorize]
        [HttpPost(Name = "PostMessage")]
        public async Task<ActionResult<Guid>> PostMessage([FromBody] PostMessageCommand postMessageCommand)
        {
            var response = await mediator.Send(postMessageCommand);
            return Ok(response);
        }
    }
}
