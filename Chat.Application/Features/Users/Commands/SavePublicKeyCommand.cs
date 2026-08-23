using MediatR;

namespace Chat.Application.Features.Users.Commands
{
    public class SavePublicKeyCommand : IRequest
    {
        public string PublicKey { get; set; } = string.Empty;
        public Guid UserId { get; set; }
    }
}
