using Chat.App.Infrastructure.Api;
using Chat.App.ViewModels;

namespace Chat.App.Contracts
{
    public interface IUserDataService
    {
        Task<ApiResponse<List<UserViewModel>>> GetAllUsers();
    }
}
