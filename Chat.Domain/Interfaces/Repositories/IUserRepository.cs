using Chat.Domain.Entities;

namespace Chat.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> ListAllUsers();
    }
}
