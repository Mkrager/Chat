using Chat.App.Contracts;
using Chat.App.Infrastructure.Api;
using Chat.App.Infrastructure.BaseServices;
using Chat.App.ViewModels;
using System.Text.Json;
using System.Text;

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

        public async Task<ApiResponse<string>> GetPublicKeyByUserId(Guid userId)
        {
            var response = await _httpClient.GetAsync($"user/{userId}/public-key");
            return await HandleResponse<string>(response);
        }

        public async Task<ApiResponse> SavePublicKey(string publicKey)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(new { publicKey }),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PatchAsync("user/public-key", content);

            return await HandleResponse(response);
        }
    }
}
