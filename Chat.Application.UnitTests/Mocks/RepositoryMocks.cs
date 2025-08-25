using Chat.Application.Contracts.Identity;
using Chat.Application.Contracts.Persistance;
using Chat.Application.DTOs;
using Chat.Domain.Entities;
using Moq;

namespace Chat.Application.UnitTests.Mocks
{
    public class RepositoryMocks
    {
        public static Mock<IChatRepository> GetChatRepository()
        {
            var messages = MockData.GetMessages();

            var mockChatRepository = new Mock<IChatRepository>();
            mockChatRepository
                .Setup(repo => repo.ListAllMessages(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string userId1, string userId2) =>
                    messages.Where(x =>
                        (x.CreatedBy == userId1 && x.ReceiverId == userId2) ||
                        (x.CreatedBy == userId2 && x.ReceiverId == userId1))
                    .OrderBy(x => x.CreatedDate)
                    .ToList());

            mockChatRepository.Setup(repo => repo.AddAsync(It.IsAny<Message>
                ())).ReturnsAsync(
                (Message message) =>
                {
                    message.Id = Guid.NewGuid();
                    message.CreatedBy = "35b820e5-1cea-47c8-a6a0-cedccfbda4e6";
                    messages.Add(message);
                    return message;
                });

            return mockChatRepository;
        }

        public static Mock<IUserService> GetUserService()
        {
            var users = new List<GetUserDetailsResponse>
                {
                    new GetUserDetailsResponse 
                    { 
                        UserId = "35b820e5-1cea-47c8-a6a0-cedccfbda4e6", 
                        UserName = "user1", Email = "user1@example.com", 
                        FirstName = "John", 
                        LastName = "Doe" 
                    },
                    new GetUserDetailsResponse 
                    { 
                        UserId = "d463ee4a-ad5c-4eb7-be35-3f0991bc20ad", 
                        UserName = "user2", Email = "user2@example.com", 
                        FirstName = "John2", 
                        LastName = "Doe2" 
                    }
                };

            var mockUserService = new Mock<IUserService>();

            mockUserService.Setup(repo => repo.GetUserDetailsAsync(It.IsAny<string>()))
                .ReturnsAsync((string userId) => users.First(a => a.UserId == userId));


            mockUserService.Setup(repo => repo.GetUserListAsync())
                .ReturnsAsync(users);

            mockUserService.Setup(repo => repo.GetUsersByIdsAsync(It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync((IEnumerable<string> userIds) =>
                {
                    return users.Where(u => userIds.Contains(u.UserId))
                    .Select(u => new GetUserDetailsResponse()
                    {
                        UserId = u.UserId,
                        UserName = u.UserName
                    }).ToList();
                });

            return mockUserService;
        }

        public static Mock<IAuthenticationService> GetAuthenticationService()
        {
            var mockAuthService = new Mock<IAuthenticationService>();

            mockAuthService.Setup(service => service.RegisterAsync(It.IsAny<RegistrationRequest>()))
                .ReturnsAsync((RegistrationRequest request) =>
                    new RegistrationResponse { UserId = Guid.NewGuid().ToString() });

            mockAuthService.Setup(service => service.AuthenticateAsync(It.IsAny<AuthenticationRequest>()))
                .ReturnsAsync((AuthenticationRequest request) =>
                    new AuthenticationResponse
                    {
                        Id = Guid.NewGuid().ToString(),
                        Token = "mock-jwt-token",
                        Email = request.Email,
                        UserName = "mockUser"
                    });
            return mockAuthService;
        }
    }
}
