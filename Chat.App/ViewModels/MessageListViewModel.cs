namespace Chat.App.ViewModels
{
    public class MessageListViewModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime SendDate { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string ReceiverUserId { get; set; } = string.Empty;
        public string SenderUserName { get; set; } = string.Empty;
    }
}
