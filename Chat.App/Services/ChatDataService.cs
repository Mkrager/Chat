using Chat.App.Contracts;
using Chat.App.ViewModels;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Chat.App.Services
{
    public class ChatDataService : IChatDataService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthenticationService _authenticationService;
        private readonly JsonSerializerOptions _jsonOptions;

        public ChatDataService(HttpClient httpClient, IAuthenticationService authenticationService)
        {
            _httpClient = httpClient;
            _authenticationService = authenticationService;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<Guid> PostMessage(MessageListViewModel messageViewModel)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7184/api/Chat");

                var accessToken = _authenticationService.GetAccessToken();

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                request.Content = new StringContent(JsonSerializer.Serialize(messageViewModel), Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    return JsonSerializer.Deserialize<Guid>(responseContent);
                }

                return Guid.Empty;
            }
            catch (Exception ex)
            {
                return Guid.Empty;
            }
        }

        public async Task<List<MessageListViewModel>> GetAllMessages(string userId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7184/api/Chat/{userId}");

                var accessToken = _authenticationService.GetAccessToken();

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    return JsonSerializer.Deserialize<List<MessageListViewModel>>(responseContent, _jsonOptions);
                }

                return new List<MessageListViewModel>();
            }
            catch (Exception ex)
            {
                return new List<MessageListViewModel>();
            }
        }

    }
}
