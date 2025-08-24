using Chat.App.Contracts;
using Chat.App.Services;
using Chat.App.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Chat.App.Components.Pages
{
    public partial class Chat
    {
        [Inject] public IChatDataService ChatDataService { get; set; }
        [Inject] public IUserDataService UserDataService { get; set; }
        [Inject] public IAuthenticationService AuthenticationService { get; set; }
        [Inject] public TokenService TokenService { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IChatHubService ChatHubService { get; set; }

        public ICollection<MessageListViewModel> Messages { get; set; } = new List<MessageListViewModel>();
        public ICollection<UserViewModel> Users { get; set; } = new List<UserViewModel>();

        private string? _receiverUserId;
        private string? _currentUserId;
        private string? _jwtToken;
        private string _message = string.Empty;
        private bool _initialized;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && !_initialized)
            {
                _initialized = true;

                _jwtToken = await AuthenticationService.GetToken();
                if (_jwtToken is null)
                {
                    await JSRuntime.InvokeVoidAsync("alert", "Error user is not authenticated");
                    NavigationManager.NavigateTo("/");
                    return;
                }

                Users = await UserDataService.GetAllUsers();
                _currentUserId = TokenService.GetClaimValue(_jwtToken, "uid");

                ChatHubService.OnMessageReceived += (msg) =>
                {
                    Messages.Add(msg);
                    InvokeAsync(StateHasChanged);
                };

                await ChatHubService.ConnectAsync(_jwtToken, _currentUserId!, _receiverUserId);

                StateHasChanged();
            }
        }

        private async Task HandleValidSubmit()
        {
            if (!string.IsNullOrEmpty(_receiverUserId))
            {
                await ChatHubService.SendMessageAsync(_currentUserId!, _receiverUserId, _message);
                _message = string.Empty;
            }
        }

        private async Task OpenChat(string receiverUserId)
        {
            _receiverUserId = receiverUserId;
            Messages = await ChatDataService.GetAllMessages(_currentUserId!, _receiverUserId);
            await ChatHubService.ConnectAsync(_jwtToken!, _currentUserId!, _receiverUserId);
        }

        private async Task Logout()
        {
            await ChatHubService.DisconnectAsync();
            await AuthenticationService.Logout();
            NavigationManager.NavigateTo("/");
        }
    }
}