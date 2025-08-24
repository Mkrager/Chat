using Chat.App.Services;
using Chat.App.ViewModels;

namespace Chat.App.Contracts
{
    public interface IAuthenticationService
    {
        Task<ApiResponse<bool>> Authenticate(AuthenticateRequest request);
        Task<ApiResponse<bool>> Register(RegistrationRequest request);
        Task Logout();
        string GetAccessToken();
    }
}