using Chat.Application.Contracts.Persistance;
using Chat.Domain.Entities;
using Moq;

namespace Chat.Application.UnitTests.Mocks
{
    public class UserServiceMock
    {
        public static Mock<IUserRepository> GetUserService()
        {
            var users = new List<User>
                {
                    new User
                    {
                        Id = Guid.Parse("35b820e5-1cea-47c8-a6a0-cedccfbda4e6"),
                        Username = "user1", 
                        Email = "user1@example.com",
                    },
                    new User
                    {
                        Id = Guid.Parse("d463ee4a-ad5c-4eb7-be35-3f0991bc20ad"),
                        Username = "user2", 
                        Email = "user2@example.com",
                    }
                };

            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(r => r.ListAllAsync())
                .ReturnsAsync(users);

            mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid id) => users.FirstOrDefault(x => x.Id == id));

            mockRepository.Setup(r => r.GetUsersByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync((IEnumerable<Guid> userIds) => users.Where(u => userIds.Contains(u.Id)).ToList());

            mockRepository.Setup(r => r.AddAsync(It.IsAny<User>()))
                .ReturnsAsync((User user) =>
                {
                    users.Add(user);
                    return user;
                });

            return mockRepository;
        }
    }
}