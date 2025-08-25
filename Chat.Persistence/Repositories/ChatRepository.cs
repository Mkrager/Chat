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

        public async Task<List<Message>> ListAllMessages(string userId1, string userId2)
        {
            var messages = await _dbContext.Messages
                .Where(x => (x.CreatedBy == userId1 && x.ReceiverId == userId2) ||
                            (x.CreatedBy == userId2 && x.ReceiverId == userId1))
                .OrderBy(x => x.CreatedDate)
                .ToListAsync();

            return messages;
        }

    }
}