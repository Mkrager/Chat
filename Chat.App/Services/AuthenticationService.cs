using Blazored.LocalStorage;
using Chat.App.Contracts;
using Chat.App.ViewModels;
using System.Text;
using System.Text.Json;

namespace Chat.App.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILocalStorageService _storageService;
        private readonly HttpClient _httpClient;


        public AuthenticationService(ILocalStorageService storageService, HttpClient httpClient)
        {
            _storageService = storageService;
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<bool>> Authenticate(string email, string password)
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

                        return new ApiResponse<bool>(System.Net.HttpStatusCode.OK, true);
                    }
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                Dictionary<string, string> errorMessages;

                try
                {
                    errorMessages = JsonSerializer.Deserialize<Dictionary<string, string>>(errorContent);
                }
                catch (JsonException)
                {
                    errorMessages = new Dictionary<string, string> { { "error", errorContent } };
                }

                return new ApiResponse<bool>(System.Net.HttpStatusCode.BadRequest, false, errorMessages.GetValueOrDefault("error"));
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(System.Net.HttpStatusCode.BadRequest, false, ex.Message);
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
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7184/api/account/details");

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var user = JsonSerializer.Deserialize<UserViewModel>(responseContent);
                    return user;
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

        public async Task<ApiResponse<bool>> Register(string firstName, string lastName, string userName, string email, string password)
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
                    return new ApiResponse<bool>(System.Net.HttpStatusCode.OK, true);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                Dictionary<string, string> errorMessages;

                try
                {
                    errorMessages = JsonSerializer.Deserialize<Dictionary<string, string>>(errorContent);
                }
                catch (JsonException)
                {
                    errorMessages = new Dictionary<string, string> { { "error", errorContent } };
                }

                return new ApiResponse<bool>(System.Net.HttpStatusCode.BadRequest, false, errorMessages.GetValueOrDefault("error"));
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(System.Net.HttpStatusCode.BadRequest, false, ex.Message);
            }
        }
    }
}
