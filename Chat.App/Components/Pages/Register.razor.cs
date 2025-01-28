using Chat.App.Contracts;
using Chat.App.Services;
using Chat.App.ViewModels;
using Microsoft.AspNetCore.Components;

namespace Chat.App.Components.Pages
{
    public partial class Register
    {
        public RegistrationRequest RegisterViewModel { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public string Message { get; set; }

        [Inject]
        private IAuthenticationService AuthenticationService { get; set; }

        public Register()
        {

        }
        protected override void OnInitialized()
        {
            RegisterViewModel = new RegistrationRequest();
        }

        protected async void HandleValidSubmit()
        {
            var result = await AuthenticationService.Register(RegisterViewModel.FirstName, RegisterViewModel.LastName, RegisterViewModel.UserName, RegisterViewModel.Email, RegisterViewModel.Password);

            if (result.Code == System.Net.HttpStatusCode.OK)
            {
                NavigationManager.NavigateTo("/login");
            }
            else
            {
                Message = NotificationFormatter.FormatErrorMessage(result.Message);
            }
            StateHasChanged();
        }

    }
}