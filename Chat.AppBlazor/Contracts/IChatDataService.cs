using Chat.App.ViewModels;

namespace Chat.App.Contracts
{
    public interface IChatDataService
    {
        Task<List<MessageListViewModel>> GetAllMessages(string userId1, string userId2);
        Task<Guid> PostMessage(MessageListViewModel messageListViewModel);

    }
}
