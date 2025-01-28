namespace Chat.Application.Features.Chat.Queries.GetMessageList
{
    public class MessageListVm
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime SendDate { get; set; }
        public string UserId { get; set; } = string.Empty;
        public MessageUserDto User { get; set; } = default!;
    }
}
