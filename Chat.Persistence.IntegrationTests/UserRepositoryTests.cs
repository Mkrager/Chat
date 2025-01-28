using Chat.Domain.Entities;
using Chat.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Chat.Persistence.IntegrationTests
{
    public class UserRepositoryTests
    {
        private readonly ChatDbContext _dbContext;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ChatDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ChatDbContext(options);
            _repository = new UserRepository(_dbContext);
        }

        [Fact]
        public async Task ListAllUsers_ShouldReturnAllMessages()
        {
            // Arrange
            var users = new List<User>
            {
                new User 
                {
                    Email = "Test@gmail.com",
                    FirstName = "TestFirstName",
                    LastName = "TestLastName",
                    UserId = "35b820e5-1cea-47c8-a6a0-cedccfbda4e6",
                    UserName = "TestUserName" 
                },

                new User
                {
                    Email = "Test2@gmail.com",
                    FirstName = "Test2FirstName",
                    LastName = "Test2LastName",
                    UserId = "d463ee4a-ad5c-4eb7-be35-3f0991bc20ad",
                    UserName = "Test2UserName"
                }
            };
            await _dbContext.Users.AddRangeAsync(users);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.ListAllUsers();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, m => m.UserName == "TestUserName");
            Assert.Contains(result, m => m.UserName == "Test2UserName");
        }

    }
}
