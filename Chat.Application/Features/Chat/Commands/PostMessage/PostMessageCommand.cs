using MediatR;

namespace Chat.Application.Features.Chat.Commands.PostMessage
{
    public class PostMessageCommand : IRequest<Guid>
    {
        public string Content { get; set; } = string.Empty;
        public string ReceiverId { get; set; } = string.Empty;
    }
}
