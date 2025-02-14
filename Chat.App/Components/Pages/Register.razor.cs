using Chat.App.Contracts;
using Chat.App.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Chat.App.Components.Pages
{
    public partial class Register
    {
        public RegistrationRequest RegisterViewModel { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        public string Message { get; set; }

        [Inject]    
        private IAuthenticationService AuthenticationService { get; set; }

        public Register()
        {
            RegisterViewModel = new RegistrationRequest();
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
            var result = await AuthenticationService.Register(RegisterViewModel.FirstName, RegisterViewModel.LastName, RegisterViewModel.UserName, RegisterViewModel.Email, RegisterViewModel.Password);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                NavigationManager.NavigateTo("/login");
            }
            else
            {
                Message = result.ErrorText;
            }
            StateHasChanged();
        }
    }
}