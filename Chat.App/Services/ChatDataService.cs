using Chat.App.Contracts;
using Chat.App.Infrastructure.Api;
using Chat.App.Infrastructure.BaseServices;
using Chat.App.ViewModels;
using System.Text;
using System.Text.Json;

namespace Chat.App.Services
{
    public class ChatDataService : BaseDataService, IChatDataService
    {
        public ChatDataService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public async Task<ApiResponse<Guid>> PostMessage(MessageListViewModel messageViewModel)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(messageViewModel),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("chat", content);
            return await HandleResponse<Guid>(response);
        }

        public async Task<ApiResponse<List<MessageListViewModel>>> GetAllMessages(Guid userId)
        {
            var response = await _httpClient.GetAsync($"chat/{userId}");
            return await HandleResponse<List<MessageListViewModel>>(response);
        }
    }
}
