using Chat.App.ViewModels;

namespace Chat.App.Contracts
{
    public interface IUserDataService
    {
        Task<List<UserViewModel>> GetAllUsers();
    }
}
