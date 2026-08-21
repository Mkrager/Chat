using Chat.App.ViewModels;

namespace Chat.App.Contracts
{
    public interface IChatDataService
    {
        Task<List<MessageListViewModel>> GetAllMessages(Guid userId);
        Task<Guid> PostMessage(MessageListViewModel messageListViewModel);

    }
}
