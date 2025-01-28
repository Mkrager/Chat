using AutoMapper;
using Chat.App.Contracts;
using Chat.App.ViewModels;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Chat.App.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly IAuthenticationService _authenticationService;


        public UserDataService(IMapper mapper, HttpClient httpClient, IAuthenticationService authenticationService)
        {
            _mapper = mapper;
            _httpClient = httpClient;
            _authenticationService = authenticationService;
        }

        public async Task<List<UserViewModel>> GetAllUsers()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7184/api/User");

            var accessToken = await _authenticationService.GetToken();

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                var users = JsonSerializer.Deserialize<List<UserViewModel>>(responseContent);

                return _mapper.Map<List<UserViewModel>>(users);
            }

            return new List<UserViewModel>();
        }
    }
}
