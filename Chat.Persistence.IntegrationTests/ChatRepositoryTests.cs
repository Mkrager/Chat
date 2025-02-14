using Microsoft.EntityFrameworkCore;
using Chat.Domain.Entities;
using Chat.Persistence.Repositories;

namespace Chat.Persistence.IntegrationTests
{
    public class ChatRepositoryTests
    {
        private readonly ChatDbContext _dbContext;
        private readonly ChatRepository _repository;

        public ChatRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ChatDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ChatDbContext(options);
            _repository = new ChatRepository(_dbContext);
        }

        [Fact]
        public async Task ListAllMessages_ShouldReturnAllMessages()
        {
            // Arrange
            var messages = new List<Message>
            {
                new Message { Id = Guid.NewGuid(), Content = "Test message 1", UserId = "2534523", SendDate = DateTime.Now, ReceiverUserId ="242432423" },
                new Message { Id = Guid.NewGuid(), Content = "Test message 2", UserId = "2534523", SendDate = DateTime.Now, ReceiverUserId = "242432423" }
            };
            await _dbContext.Messages.AddRangeAsync(messages);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.ListAllMessages("2534523", "242432423");

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, m => m.Content == "Test message 1");
            Assert.Contains(result, m => m.Content == "Test message 2");
        }

        [Fact]
        public async Task PostMessage_ShouldAddMessageToDatabase()
        {
            // Arrange
            var message = new Message { Content = "New message", UserId = "53456345645", SendDate = DateTime.Now };

            // Act
            var result = await _repository.PostMessage(message);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New message", result.Content);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(1, _dbContext.Messages.Count());
        }
    }
}
