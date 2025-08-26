using Chat.Application.DTOs;
using MediatR;

namespace Chat.Application.Features.Account.Queries.Authentication
{
    public class AuthenticationQuery : IRequest<AuthenticationVm>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
