using Chat.Application.Features.Account.Command.Authentication;
using Chat.Application.Features.Account.Queries.GetUserDetails;
using Chat.Application.Features.Account.Queries.Registration;
using Chat.Domain.Models;
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
            var dtos = await mediator.Send(new AuthenticationCommand() { Email = request.Email, Password = request.Password});
            return Ok(dtos);
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegistrationResponse>> RegisterAsync(RegistrationRequest request)
        {
            var dtos = await mediator.Send(new RegistrationQuery() { UserName = request.UserName, Password = request.Password, Email = request.Email, FirstName = request.FirstName, LastName = request.LastName});
            return Ok(dtos);
        }

        [HttpGet]
        public async Task<ActionResult<GetUserDetailsResponse>> GetUserDetails()
        {
            var dtos = await mediator.Send(new GetUserDetailsQuery());
            return Ok(dtos);
        }
    }
}
