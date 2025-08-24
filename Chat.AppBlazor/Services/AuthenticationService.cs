using Blazored.LocalStorage;
using Chat.App.ViewModels;
using System.Text;
using System.Text.Json;
using System.Net;
using Chat.App.Contracts;

namespace Chat.App.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILocalStorageService _storageService;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        public AuthenticationService(ILocalStorageService storageService, HttpClient httpClient)
        {
            _storageService = storageService;
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<ApiResponse<bool>> Authenticate(AuthenticateRequest request)
        {
            try
            {
                var authenticationRequest = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7184/api/Account/authenticate")
                {
                    Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
                };

                var response = await _httpClient.SendAsync(authenticationRequest);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var authenticationResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent, _jsonOptions);

                    var jwtToken = authenticationResponse?.Token;

                    if (!string.IsNullOrEmpty(jwtToken))
                    {
                        await _storageService.SetItemAsync("access_token", jwtToken);
                        return new ApiResponse<bool>(HttpStatusCode.OK, true);
                    }
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                var errorMessages = JsonSerializer.Deserialize<Dictionary<string, string>>(errorContent) ??
                                    new Dictionary<string, string> { { "error", errorContent } };

                return new ApiResponse<bool>(HttpStatusCode.BadRequest, false, errorMessages.GetValueOrDefault("error"));
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(HttpStatusCode.BadRequest, false, ex.Message);
            }
        }


        public async Task<string> GetToken()
        {
            return await _storageService.GetItemAsStringAsync("access_token");
        }

        public async Task Logout()
        {
            await _storageService.RemoveItemsAsync(new[] { "access_token" });
        }

        public async Task<ApiResponse<bool>> Register(RegistrationRequest request)
        {
            try
            {
                var registerRequest = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7184/api/account/register")
                {
                    Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
                };

                var response = await _httpClient.SendAsync(registerRequest);

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<bool>(HttpStatusCode.OK, true);
                }

                var errorContent = await response.Content.ReadAsStringAsync();

                var errorMessages = JsonSerializer.Deserialize<Dictionary<string, string>>(errorContent) ??
                                    new Dictionary<string, string> { { "error", errorContent } };

                return new ApiResponse<bool>(HttpStatusCode.BadRequest, false, errorMessages.GetValueOrDefault("error"));
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(HttpStatusCode.BadRequest, false, ex.Message);
            }
        }
    }
}
