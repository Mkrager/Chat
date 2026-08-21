namespace Chat.App.ViewModels
{
    public class MessageListViewModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public Guid ReceiverUserId { get; set; }
        public string SenderUsername { get; set; } = string.Empty;
    }
}
