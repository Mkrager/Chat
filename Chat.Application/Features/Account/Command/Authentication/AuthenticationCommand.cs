using Chat.Domain.Models;
using MediatR;

namespace Chat.Application.Features.Account.Command.Authentication
{
    public class AuthenticationCommand : IRequest<AuthenticationResponse>
    {
        public string Email { get; set; } = string.Empty;
        public string Password {  get; set; } = string.Empty;
    }
}
