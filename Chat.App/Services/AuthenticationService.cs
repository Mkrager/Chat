using AutoMapper;
using Blazored.LocalStorage;
using Chat.App.Contracts;
using Chat.App.ViewModels;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Chat.App.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMapper _mapper;
        private readonly ILocalStorageService _storageService;
        private readonly TokenService _tokenService;
        private readonly HttpClient _httpClient;


        public AuthenticationService(IMapper mapper, TokenService tokenService, ILocalStorageService storageService, HttpClient httpClient)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _storageService = storageService;
            _httpClient = httpClient;
        }

        public async Task<ApiResponse> Authenticate(string email, string password)
        {
            try
            {
                var authenticationRequest = new LoginResponse() { Email = email, Password = password };


                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7184/api/account/authenticate")
                {
                    Content = new StringContent(JsonSerializer.Serialize(authenticationRequest), Encoding.UTF8, "application/json")
                };

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var authenticationResponse = JsonSerializer.Deserialize<LoginRequest>(responseContent);

                    var jwtToken = authenticationResponse?.Token;

                    if (!string.IsNullOrEmpty(jwtToken))
                    {
                        await _storageService.SetItemAsync("access_token", jwtToken);

                        return new ApiResponse { Code = response.StatusCode };
                    }
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return new ApiResponse() { Message = errorContent };

            }
            catch (Exception ex)
            {
                return new ApiResponse() { Message = ex.Message };
            }
        }


        public async Task<string> GetToken()
        {
            var tokenJson = await _storageService.GetItemAsStringAsync("access_token");
            if (tokenJson == null)
            {
                return null;
            }
            else
                return tokenJson.Trim('"');
        }

        public async Task<UserViewModel> GetUserDetails()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://your-api-url.com/api/account/details");

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var user = JsonSerializer.Deserialize<UserViewModel>(responseContent);
                    return _mapper.Map<UserViewModel>(user);
                }

                var errorResponse = await response.Content.ReadAsStringAsync();
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task Logout()
        {
            await _storageService.RemoveItemsAsync(new[] { "access_token" });
        }

        public async Task<ApiResponse> Register(string firstName, string lastName, string userName, string email, string password)
        {
            try
            {
                var registrationRequest = new RegistrationRequest() { Password = password, Email = email, FirstName = firstName, LastName = lastName, UserName = userName };

                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7184/api/account/register")
                {
                    Content = new StringContent(JsonSerializer.Serialize(registrationRequest), Encoding.UTF8, "application/json")
                };

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse { Code = response.StatusCode };
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return new ApiResponse() { Message = errorContent };
            }
            catch (Exception ex)
            {
                return new ApiResponse();
            }
        }
    }
}
