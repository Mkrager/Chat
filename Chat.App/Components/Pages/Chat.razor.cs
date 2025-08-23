using Chat.App.Contracts;
using Chat.App.Services;
using Chat.App.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

namespace Chat.App.Components.Pages
{
    public partial class Chat
    {
        [Inject]
        public IChatDataService ChatDataService { get; set; }

        [Inject]
        public IUserDataService UserDataService { get; set; }

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        [Inject]
        public TokenService TokenService { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public ICollection<MessageListViewModel> Messages { get; set; } = new List<MessageListViewModel>();
        public ICollection<UserViewModel> Users { get; set; } = new List<UserViewModel>();

        private string _receiverUserId = null;
        private string _currentUserId = null;
        private string _userName = null;

        private HubConnection? _hubConnection;
        private string _message = string.Empty;
        private string jwtToken;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                jwtToken = await AuthenticationService.GetToken();
                if (jwtToken == null)
                {
                    await JSRuntime.InvokeVoidAsync("alert", "Error user is not authenticated");
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    var userId = TokenService.GetClaimValue(jwtToken, "uid");
                    var userName = TokenService.GetClaimValue(jwtToken, "Sub");
                    _userName = userName;
                    _currentUserId = userId;

                    if (!string.IsNullOrEmpty(_receiverUserId) && !string.IsNullOrEmpty(_currentUserId))
                    {
                        Messages = await ChatDataService.GetAllMessages(_currentUserId, _receiverUserId);
                    }

                    Users = await UserDataService.GetAllUsers();

                    _hubConnection = new HubConnectionBuilder()
                        .WithUrl("https://localhost:7184/chatHub", options =>
                        {
                            options.AccessTokenProvider = async () =>
                            {
                                return jwtToken;
                            };
                        })
                        .Build();

                    _hubConnection.On<string, string>("SendMessage", (senderName, message) =>
                    {
                        var newMessage = new MessageListViewModel
                        {
                            SenderUserName = senderName,
                            Content = message,
                            SendDate = DateTime.Now
                        };

                        Messages.Add(newMessage);
                        InvokeAsync(StateHasChanged);
                    });

                    await _hubConnection.StartAsync();

                    if (!string.IsNullOrEmpty(_receiverUserId) && !string.IsNullOrEmpty(_currentUserId))
                    {
                        var groupName = string.Join("_", new[] { _currentUserId, _receiverUserId }.OrderBy(id => id));
                        await _hubConnection.SendAsync("JoinGroup", groupName);
                    }

                }
            }
            StateHasChanged();
        }

        protected async Task HandleValidSubmit()
        {
            if (_hubConnection is not null)
            {
                var groupName = string.Join("_", new[] { _currentUserId, _receiverUserId }.OrderBy(id => id));
                await _hubConnection.SendAsync("SendMessageToGroup", groupName, _userName, _message, _receiverUserId);
            }
        }

        private async Task OpenChat(string receiverUserId)
        {
            _receiverUserId = receiverUserId;

            Messages = await ChatDataService.GetAllMessages(_currentUserId, _receiverUserId);

            if (_hubConnection is not null)
            {
                var groupName = string.Join("_", new[] { _currentUserId, _receiverUserId }.OrderBy(id => id));
                await _hubConnection.SendAsync("JoinGroup", groupName);
            }
        }

        private async Task Logout()
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.StopAsync();
                await _hubConnection.DisposeAsync();
                _hubConnection = null;
            }

            await AuthenticationService.Logout();
            NavigationManager.NavigateTo("/");
        }

    }
}