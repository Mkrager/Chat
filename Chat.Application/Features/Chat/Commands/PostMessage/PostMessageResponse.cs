namespace Chat.Application.Features.Chat.Commands.PostMessage
{
    public class PostMessageResponse
    {
        public string SenderUserName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }
}
