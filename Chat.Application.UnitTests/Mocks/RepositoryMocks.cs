using Chat.Application.ChatHub;
using Chat.Application.Contracts.Identity;
using Chat.Application.Contracts.Persistance;
using Chat.Application.DTOs;
using Chat.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
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
                        (x.UserId == userId1 && x.ReceiverUserId == userId2) ||
                        (x.UserId == userId2 && x.ReceiverUserId == userId1))
                    .OrderBy(x => x.SendDate)
        .ToList());

            mockChatRepository.Setup(repo => repo.PostMessage(It.IsAny<Message>
                ())).ReturnsAsync(
                (Message message) =>
                {
                    message.Id = Guid.NewGuid();
                    messages.Add(message);
                    return message;
                });

            return mockChatRepository;
        }

        public static Mock<IHubContext<ChatHubs>> GetHubContext()
        {
            var mockHubContext = new Mock<IHubContext<ChatHubs>>();
            var mockClients = new Mock<IHubClients>();
            var mockClientProxy = new Mock<IClientProxy>();

            mockClientProxy
                .Setup(proxy => proxy.SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            mockClients
                .Setup(clients => clients.Group(It.IsAny<string>()))
                .Returns(mockClientProxy.Object);

            mockHubContext
                .Setup(context => context.Clients)
                .Returns(mockClients.Object);

            return mockHubContext;
        }

        public static Mock<IUserService> GetUserService()
        {
            var users = new List<GetUserDetailsResponse>
                {
                    new GetUserDetailsResponse { UserId = "35b820e5-1cea-47c8-a6a0-cedccfbda4e6", UserName = "user1", Email = "user1@example.com", FirstName = "John", LastName = "Doe" },
                    new GetUserDetailsResponse { UserId = "35b820e5-1cea-47c8-a6a0-cedccfbda4e1", UserName = "user2", Email = "user2@example.com", FirstName = "John2", LastName = "Doe2" }
                };

            var mockUserService = new Mock<IUserService>();

            mockUserService
                .Setup(repo => repo.GetUserDetailsAsync())
                .ReturnsAsync(users.First());

            mockUserService
                .Setup(repo => repo.GetUserListAsync())
                .ReturnsAsync(users);

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
