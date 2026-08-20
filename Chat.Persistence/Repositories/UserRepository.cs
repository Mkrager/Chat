using Chat.Application.Contracts;
using Chat.Application.Contracts.Persistance;
using Chat.Application.DTOs;
using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
    }
}
