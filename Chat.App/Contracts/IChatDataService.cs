using Chat.App.Infrastructure.Api;
using Chat.App.ViewModels;

namespace Chat.App.Contracts
{
    public interface IChatDataService
    {
        Task<ApiResponse<List<MessageListViewModel>>> GetAllMessages(Guid userId, int page, int pageSize);
        Task<ApiResponse<Guid>> PostMessage(MessageListViewModel messageListViewModel);

    }
}
