using Chat.Application.DTOs;
using Chat.Application.Features.Users.GetUserList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IMediator mediator) : Controller
    {
        [Authorize]
        [HttpGet(Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<GetUserDetailsResponse>>> GetAllUsers()
        {
            var dtos = await mediator.Send(new GetUserListQuery());
            return Ok(dtos);
        }

    }
}
