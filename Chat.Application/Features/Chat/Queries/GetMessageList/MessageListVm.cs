namespace Chat.Application.Features.Chat.Queries.GetMessageList
{
    public class MessageListVm
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string ReceiverId { get; set; } = string.Empty;
        public string SenderUserName { get; set; } = string.Empty;
    }
}
