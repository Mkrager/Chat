using Chat.Domain.Entities;

namespace Chat.Application.Contracts.Persistance
{
    public interface IChatRepository
    {
        Task<Message> PostMessage(Message message);
        Task<List<Message>> ListAllMessages(string userId, string receiverUserId);
    }
}
