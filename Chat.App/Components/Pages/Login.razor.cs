using Chat.App.Contracts;
using Chat.App.ViewModels;
using Microsoft.AspNetCore.Components;
using Chat.App.Services;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;

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
            LoginViewModel = new LoginResponse();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                string jwtToken = await AuthenticationService.GetToken();
                if (jwtToken != null)
                {
                    await JSRuntime.InvokeVoidAsync("alert", "Error user already authenticated");
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                }
            }
        }

        protected async void HandleValidSubmit()
        {
            var result = await AuthenticationService.Authenticate(LoginViewModel.Email, LoginViewModel.Password);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                NavigationManager.NavigateTo("/chat");
            }
            else
            {
                Message = result.ErrorText;
            }
            StateHasChanged();
        }
    }
}
