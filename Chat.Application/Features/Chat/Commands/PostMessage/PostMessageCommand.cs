using MediatR;

namespace Chat.Application.Features.Chat.Commands.PostMessage
{
    public class PostMessageCommand : IRequest<Guid>
    {
        public string Content { get; set; } = string.Empty;
        public DateTime SendDate { get; set; }
        public string ReceiverUserId { get; set; } = string.Empty;
    }
}
