using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Persistence.IntegrationTests
{
    public class DbContextTests
    {
        private readonly ChatDbContext _dbContext;
        public DbContextTests()
        {
            var options = new DbContextOptionsBuilder<ChatDbContext>()
            .UseInMemoryDatabase("TestDatabase")
                .Options;

            _dbContext = new ChatDbContext(options);
        }

        [Fact]
        public async Task SaveChangesAsync_ShouldSetCreatedDate_WhenEntityIsAdded()
        {
            var message = new Message { Content = "Test" };

            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();

            var savedMessage = await _dbContext.Messages
                .FirstOrDefaultAsync(m => m.Content == "Test");

            Assert.NotNull(savedMessage);
            Assert.NotEqual(DateTime.MinValue, savedMessage.SendDate);
        }
    }
}
