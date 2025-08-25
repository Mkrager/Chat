namespace Chat.App.ViewModels
{
    public class ChatViewModel
    {
        public List<MessageListViewModel>? Messages { get; set; } = default!;
        public List<UserViewModel> Users { get; set; } = default!;
        public string JwtToken { get; set; } = string.Empty;
    }
}
