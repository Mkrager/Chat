using Chat.Application.Contracts.Persistance;
using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Persistence.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ChatDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<User>> GetUsersByIdsAsync(IEnumerable<Guid> userIds)
        {
            return await _dbContext.Users
                 .Where(u => userIds.Contains(u.Id)).ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(r => r.Email == email);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(r => r.Username == username);
        }

    }
}
