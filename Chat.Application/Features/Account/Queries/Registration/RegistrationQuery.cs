using Chat.Domain.Models;
using MediatR;

namespace Chat.Application.Features.Account.Queries.Registration
{
    public class RegistrationQuery : IRequest<RegistrationResponse>
    {
        public string UserId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

    }
}
