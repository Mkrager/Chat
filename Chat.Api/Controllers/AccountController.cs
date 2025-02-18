using Chat.Application.DTOs;
using Chat.Application.Features.Account.Command.Registration;
using Chat.Application.Features.Account.Queries.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IMediator mediator) : Controller
    {

        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            var dtos = await mediator.Send(new AuthenticationQuery() { Email = request.Email, Password = request.Password });
            return Ok(dtos);
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegistrationResponse>> RegisterAsync(RegistrationRequest request)
        {
            var dtos = await mediator.Send(new RegistrationCommand() { UserName = request.UserName, Password = request.Password, Email = request.Email, FirstName = request.FirstName, LastName = request.LastName });
            return Ok(dtos);
        }
    }
}
