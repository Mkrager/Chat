using Chat.Domain.Entities;
using Chat.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Chat.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        protected readonly ChatDbContext _dbContext;
        public UserRepository(ChatDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<User>> ListAllUsers()
        {
            var users = await _dbContext.Users.ToListAsync();
            return users;
        }
    }
}
