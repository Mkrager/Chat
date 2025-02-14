using Chat.Application.Contracts.Persistance;
using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Persistence.Repositories
{
    public class ChatRepository : IChatRepository
    {
        protected readonly ChatDbContext _dbContext;
        public ChatRepository(ChatDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Message>> ListAllMessages(string userId1, string userId2)
        {
            var messages = await _dbContext.Messages
                .Where(x => (x.UserId == userId1 && x.ReceiverUserId == userId2) ||
                            (x.UserId == userId2 && x.ReceiverUserId == userId1))
                .OrderBy(x => x.SendDate)
                .ToListAsync();

            return messages;
        }      

        public async Task<Message> PostMessage(Message message)
        {
            await _dbContext.Messages.AddAsync(message);
            await _dbContext.SaveChangesAsync();
            return message;
        }
    }
}
