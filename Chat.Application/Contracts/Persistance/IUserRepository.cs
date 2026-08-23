using Chat.Domain.Entities;

namespace Chat.Application.Contracts.Persistance
{
    public interface IUserRepository : IAsyncRepository<User>
    {
        Task<List<User>> GetUsersByIdsAsync(IEnumerable<Guid> userIds);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task SavePublicKey(User user, string publicKey);
    }
}
