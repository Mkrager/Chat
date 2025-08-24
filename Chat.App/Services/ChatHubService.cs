using Chat.App.Contracts;
using Chat.App.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;

namespace Chat.App.Services
{
    public class ChatHubService : IChatHubService
    {
        private HubConnection? _hubConnection;
        public event Action<MessageListViewModel>? OnMessageReceived;

        public async Task ConnectAsync(string token, string currentUserId, string? receiverUserId)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7184/chatHub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token)!;
                })
                .Build();

            _hubConnection.On<string>("SendMessage", (message) =>
            {
                var newMessage = new MessageListViewModel { Content = message };
                OnMessageReceived?.Invoke(newMessage);
            });

            await _hubConnection.StartAsync();

            if (!string.IsNullOrEmpty(receiverUserId))
            {
                var groupName = string.Join("_", new[] { currentUserId, receiverUserId }.OrderBy(id => id));
                await _hubConnection.SendAsync("JoinGroup", groupName);
            }
        }

        public async Task SendMessageAsync(string currentUserId, string receiverUserId, string message)
        {
            if (_hubConnection is null) return;
            var groupName = string.Join("_", new[] { currentUserId, receiverUserId }.OrderBy(id => id));
            await _hubConnection.SendAsync("SendMessageToGroup", groupName, message, receiverUserId);
        }

        public async Task DisconnectAsync()
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.StopAsync();
                await _hubConnection.DisposeAsync();
                _hubConnection = null;
            }
        }
    }
}
