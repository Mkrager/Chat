using System.Net;
using System.Text;
using System.Text.Json;
using Chat.Api.IntegrationTests.Base;
using Chat.Application.Features.Chat.Commands.PostMessage;
using Chat.Application.Features.Chat.Queries.GetMessageList;

namespace Chat.Api.IntegrationTests.Controllers
{
    public class ChatControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        public ChatControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAllMessages_ReturnsSuccessAndNonEmptyList()
        {
            var client = _factory.GetAuthenticatedClient();
            string userId = "d385ac98-8c90-4946-9ab3-27f821fd7623";
            string receiverUserId = "6e02e7bd-8f2e-4c25-9696-dad78a1307cb";
            var response = await client.GetAsync($"/api/Chat/{userId}/{receiverUserId}");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<MessageListVm>>(responseString);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(result.Count > 0);
        }


        [Fact]
        public async Task PostMessage_ReturnsSuccessAndValidResponse()
        {
            var client = _factory.GetAuthenticatedClient();

            var postMessageCommand = new PostMessageCommand
            {
                Content = "Hi",
                ReceiverUserId = "6e02e7bd-8f2e-4c25-9696-dad78a1307cb"
            };

            var content = new StringContent(
                JsonSerializer.Serialize(postMessageCommand),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("/api/Chat", content);

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Guid>(responseString);

            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
