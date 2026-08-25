using MediatR;

namespace Chat.Application.Features.Chat.Commands.PostMessage
{
    public class PostMessageCommand : IRequest<PostMessageResponse>
    {
        public string Ciphertext { get; set; } = string.Empty;
        public string Iv { get; set; } = string.Empty;
        public string ReceiverId { get; set; } = string.Empty;
    }
}

