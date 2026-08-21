using Chat.App.Infrastructure.Api;
using Chat.App.ViewModels;

namespace Chat.App.Contracts
{
    public interface IAuthenticationService
    {
        Task<ApiResponse> Authenticate(AuthenticateRequest request);
        Task<ApiResponse> Register(RegistrationRequest request);
        Task Logout();
        string GetAccessToken();
    }
}