using Chat.App.Contracts;
using Chat.App.Infrastructure.Api;
using Chat.App.Infrastructure.BaseServices;
using Chat.App.ViewModels;

namespace Chat.App.Services
{
    public class UserDataService : BaseDataService, IUserDataService
    {
        public UserDataService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public async Task<ApiResponse<List<UserViewModel>>> GetAllUsers()
        {
            var response = await _httpClient.GetAsync("user");
            return await HandleResponse<List<UserViewModel>>(response);
        }
    }
}
