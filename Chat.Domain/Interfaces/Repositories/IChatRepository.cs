using Chat.Domain.Entities;

namespace Chat.Domain.Interfaces.Repositories
{
    public interface IChatRepository
    {
        Task<Message> PostMessage(Message message);
        Task<List<Message>> ListAllMessages(string userId, string receiverUserId);
    }
}
