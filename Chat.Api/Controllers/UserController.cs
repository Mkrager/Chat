using Chat.Application.Contracts;
using Chat.Application.Features.Users.Commands;
using Chat.Application.Features.Users.Queris;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IMediator mediator, ICurrentUserService currentUserService) : Controller
    {
        [Authorize]
        [HttpGet(Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<UserListVm>>> GetAllUsers()
        {
            var dtos = await mediator.Send(new GetUserListQuery());
            return Ok(dtos);
        }

        [HttpPatch("public-key", Name = "SavePublicKey")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> SavePublicKey([FromBody] SavePublicKeyCommand savePublicKeyCommand)
        {
            savePublicKeyCommand.UserId = currentUserService.UserId.Value;

            await mediator.Send(savePublicKeyCommand);

            return NoContent();
        }
    }
}
