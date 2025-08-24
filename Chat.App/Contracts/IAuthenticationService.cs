using Chat.App.Services;
using Chat.App.ViewModels;

namespace Chat.App.Contracts
{
    public interface IAuthenticationService
    {
        Task<ApiResponse<bool>> Authenticate(AuthenticateRequest authenticateRequest);
        Task<ApiResponse<bool>> Register(RegistrationRequest registrationRequest);
        Task<string> GetToken();
        Task Logout();
    }
}
