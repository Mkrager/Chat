using Chat.Domain.Entities;

namespace Chat.Application.Contracts.Persistance
{
    public interface IUserRepository : IAsyncRepository<User>
    {
        Task<List<User>> GetUsersByIdsAsync(IEnumerable<Guid> userIds);
    }
}
