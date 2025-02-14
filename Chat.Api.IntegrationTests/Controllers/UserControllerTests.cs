using Chat.Api.IntegrationTests.Base;
using Chat.Application.DTOs;
using System.Text.Json;

namespace Chat.Api.IntegrationTests.Controllers
{
    public class UserControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        public UserControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAllUsers_ReturnsSuccessAndNonEmptyList()
        {
            var client = _factory.GetAuthenticatedClient();
            var response = await client.GetAsync("/api/User");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<GetUserDetailsResponse>>(responseString);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(result.Count > 0);
        }
    }
}

