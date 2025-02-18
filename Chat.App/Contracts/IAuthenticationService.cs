using Chat.App.Services;
using Chat.App.ViewModels;

namespace Chat.App.Contracts
{
    public interface IAuthenticationService
    {
        Task<ApiResponse<bool>> Authenticate(string email, string password);
        Task<ApiResponse<bool>> Register(string firstName, string lastName, string userName, string email, string password);
        Task<string> GetToken();
        Task Logout();
        Task<UserViewModel> GetUserDetails();
    }
}
