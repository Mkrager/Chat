using Microsoft.EntityFrameworkCore;
using Chat.Domain.Entities;
using Chat.Persistence.Repositories;
using Chat.Application.Contracts;
using Moq;

namespace Chat.Persistence.IntegrationTests
{
    public class ChatRepositoryTests
    {
        private readonly ChatDbContext _dbContext;
        private readonly ChatRepository _repository;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly string _currentUserId;

        public ChatRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ChatDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _currentUserId = "12300000-0000-0000-0000-000000000000";
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _currentUserServiceMock.Setup(m => m.UserId).Returns(_currentUserId);

            _dbContext = new ChatDbContext(options, _currentUserServiceMock.Object);
            _repository = new ChatRepository(_dbContext);
        }

        [Fact]
        public async Task ListAllMessages_ShouldReturnAllMessages()
        {
            var messages = new List<Message>
            {
                new Message 
                { 
                    Id = Guid.NewGuid(), 
                    Content = "Test message 1", 
                    CreatedDate = DateTime.Now, 
                    ReceiverId ="242432423" 
                },
                new Message 
                { 
                    Id = Guid.NewGuid(), 
                    Content = "Test message 2",
                    CreatedDate = DateTime.Now, 
                    ReceiverId = "242432423"
                }
            };

            await _dbContext.Messages.AddRangeAsync(messages);
            await _dbContext.SaveChangesAsync();

            var result = await _repository.ListAllMessages("12300000-0000-0000-0000-000000000000", "242432423");

            Assert.Equal(2, result.Count);
            Assert.Contains(result, m => m.Content == "Test message 1");
            Assert.Contains(result, m => m.Content == "Test message 2");
        }
    }
}
