using Chat.App.Contracts;
using Chat.App.ViewModels;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Chat.App.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthenticationService _authenticationService;
        private readonly JsonSerializerOptions _jsonOptions;
        public UserDataService(HttpClient httpClient, IAuthenticationService authenticationService)
        {
            _httpClient = httpClient;
            _authenticationService = authenticationService;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<UserViewModel>> GetAllUsers()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7184/api/User");

            var accessToken = _authenticationService.GetAccessToken();

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<List<UserViewModel>>(responseContent, _jsonOptions);
            }

            return new List<UserViewModel>();
        }
    }
}
