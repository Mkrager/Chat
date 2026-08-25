using MediatR;

namespace Chat.Application.Features.Chat.Queries.GetMessageList
{
    public class GetMessagesListQuery : IRequest<List<MessageListVm>>
    {
        public Guid UserId { get; set; }
        public Guid ReceiverUserId { get; set; }
        public int Page {  get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
}
