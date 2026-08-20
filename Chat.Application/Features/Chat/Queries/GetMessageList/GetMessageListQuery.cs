using MediatR;

namespace Chat.Application.Features.Chat.Queries.GetMessageList
{
    public class GetMessageListQuery : IRequest<List<MessageListVm>>
    {
        public Guid UserId { get; set; }
        public Guid ReceiverUserId { get; set; }
    }
}
