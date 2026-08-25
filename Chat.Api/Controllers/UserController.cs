using Chat.Application.Contracts;
using Chat.Application.Features.Users.Commands.SavePublicKey;
using Chat.Application.Features.Users.Queries.GetPublicKeyByUser;
using Chat.Application.Features.Users.Queries.GetUserList;
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

        [Authorize]
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

        [Authorize]
        [HttpGet("{recieverId}/public-key", Name = "GetPublicKeyByUserId")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetPublicKeyByUserId(Guid recieverId)
        {
            var publicKey = await mediator.Send(new GetPublicKeyByUserQuery()
            {
                UserId = recieverId
            });

            return Ok(publicKey);
        }
    }
}
