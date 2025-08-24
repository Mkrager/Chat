using Chat.App.ViewModels;

namespace Chat.App.Contracts
{
    public interface IChatHubService
    {
        Task ConnectAsync(string token, string currentUserId, string? receiverUserId);
        Task DisconnectAsync();
        Task SendMessageAsync(string currentUserId, string receiverUserId, string message);
        event Action<MessageListViewModel> OnMessageReceived;
    }
}
