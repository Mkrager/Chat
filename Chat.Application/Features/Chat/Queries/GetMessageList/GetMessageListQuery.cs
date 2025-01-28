using MediatR;

namespace Chat.Application.Features.Chat.Queries.GetMessageList
{
    public class GetMessageListQuery : IRequest<List<MessageListVm>>
    {
        public string UserId { get; set; } = string.Empty;
        public string ReceiverUserId { get; set; } = string.Empty;
    }
}
