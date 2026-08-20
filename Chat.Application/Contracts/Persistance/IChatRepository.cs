using Chat.Domain.Entities;

namespace Chat.Application.Contracts.Persistance
{
    public interface IChatRepository : IAsyncRepository<Message>
    {
        Task<List<Message>> ListAllMessages(Guid userId, Guid receiverUserId);
    }
}
