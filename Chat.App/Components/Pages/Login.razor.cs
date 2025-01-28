using Chat.App.Contracts;
using Chat.App.ViewModels;
using Microsoft.AspNetCore.Components;
using Chat.App.Services;
using Microsoft.JSInterop;

namespace Chat.App.Components.Pages
{
    public partial class Login
    {
        public LoginResponse LoginViewModel { get; set; }
        public string Message { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        private IAuthenticationService AuthenticationService { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }


        public Login()
        {

        }
        protected override void OnInitialized()
        {
            LoginViewModel = new LoginResponse();
        }

        protected async void HandleValidSubmit()
        {
                var result = await AuthenticationService.Authenticate(LoginViewModel.Email, LoginViewModel.Password);
                if (result.Code == System.Net.HttpStatusCode.OK)
                {
                    NavigationManager.NavigateTo("/chat");
                }
                else
                {
                    Message = NotificationFormatter.FormatErrorMessage(result.Message);
                }
                StateHasChanged();
            }
        }

    }
