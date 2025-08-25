using Chat.Application.Contracts;
using Chat.Domain.Entities;
using Chat.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Chat.Persistence.IntegrationTests
{
    public class BaseRepositoryTests
    {
        private readonly ChatDbContext _dbContext;
        private readonly BaseRepository<Message> _repository;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly string _currentUserId;

        public BaseRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ChatDbContext>()
                .UseInMemoryDatabase(databaseName: "ChatDb")
                .Options;

            _currentUserId = "00000000-0000-0000-0000-000000000000";
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _currentUserServiceMock.Setup(m => m.UserId).Returns(_currentUserId);

            _dbContext = new ChatDbContext(options, _currentUserServiceMock.Object);
            _repository = new BaseRepository<Message>(_dbContext);
        }

        [Fact]
        public async Task AddAsync_ShouldAddEntityToDatabase()
        {
            var message = new Message { Content = "New message", CreatedDate = DateTime.UtcNow };

            var result = await _repository.AddAsync(message);

            var addedMessage = await _dbContext.Messages.FindAsync(result.Id);
            Assert.NotNull(addedMessage);
            Assert.Equal("New message", addedMessage.Content);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEntity()
        {
            var message = new Message { Content = "Old content", CreatedDate = DateTime.UtcNow };
            await _repository.AddAsync(message);

            message.Content = "Updated content";
            await _repository.UpdateAsync(message);

            var updatedmessage = await _dbContext.Messages.FindAsync(message.Id);
            Assert.NotNull(updatedmessage);
            Assert.Equal("Updated content", updatedmessage.Content);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteEntity()
        {
            var message = new Message { Content = "Content to Delete", CreatedDate = DateTime.UtcNow };
            await _repository.AddAsync(message);

            await _repository.DeleteAsync(message);

            var deletedmessage = await _dbContext.Messages.FindAsync(message.Id);
            Assert.Null(deletedmessage);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity_WhenEntityExists()
        {
            var message = new Message { Content = "Message", CreatedDate = DateTime.UtcNow };
            await _repository.AddAsync(message);

            var result = await _repository.GetByIdAsync(message.Id);

            Assert.NotNull(result);
            Assert.Equal(message.Content, result.Content);
        }

        [Fact]
        public async Task ListAllAsync_ShouldReturnAllEntities()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            var result = await _repository.ListAllAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }
    }
}
