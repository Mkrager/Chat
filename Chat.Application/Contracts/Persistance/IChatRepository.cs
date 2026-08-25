using Chat.Domain.Entities;

namespace Chat.Application.Contracts.Persistance
{
    public interface IChatRepository : IAsyncRepository<Message>
    {
        Task<List<Message>> ListMessages(Guid userId, Guid receiverUserId, int page, int pageSize);
    }
}
