using AutoMapper;
using Chat.App.Contracts;
using Chat.App.ViewModels;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Chat.App.Services
{
    public class ChatDataService : IChatDataService
    {
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly IAuthenticationService _authenticationService;
        public ChatDataService(IMapper mapper, HttpClient httpClient, IAuthenticationService authenticationService)
        {
            _mapper = mapper;
            _httpClient = httpClient;
            _authenticationService = authenticationService;
        }

        public async Task<Guid> PostMessage(MessageListViewModel messageViewModel)
        {
            try
            {
                var accessToken = await _authenticationService.GetToken();

                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7184/api/Chat");

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

        public async Task<List<MessageListViewModel>> GetAllMessages(string userId1, string userId2)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7184/api/Chat/{userId1}/{userId2}");

                var accessToken = await _authenticationService.GetToken();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var messages = JsonSerializer.Deserialize<List<MessageListViewModel>>(responseContent);

                    return  _mapper.Map<List<MessageListViewModel>>(messages);
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
