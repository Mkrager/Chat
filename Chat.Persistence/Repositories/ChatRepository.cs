using Chat.Application.Contracts.Persistance;
using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Persistence.Repositories
{
    public class ChatRepository : BaseRepository<Message>, IChatRepository
    {
        public ChatRepository(ChatDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Message>> ListMessages(Guid userId1, Guid userId2, int page, int pageSize)
        {
            var messages = await _dbContext.Messages
                .Where(x => (x.CreatedBy == userId1 && x.ReceiverId == userId2) ||
                            (x.CreatedBy == userId2 && x.ReceiverId == userId1))
                .OrderByDescending(x => x.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return messages;
        }

    }
}