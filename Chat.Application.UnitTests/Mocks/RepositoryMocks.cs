using Chat.Application.ChatHub;
using Chat.Domain.Entities;
using Chat.Domain.Interfaces.Identity;
using Chat.Domain.Interfaces.Repositories;
using Chat.Domain.Models;
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
        .           ToList());

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



        public static Mock<IUserRepository> GetUserRepository()
        {
            var users = MockData.GetUsers();

            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository
                .Setup(repo => repo.ListAllUsers()).ReturnsAsync(users);


            return mockUserRepository;
        }

        public static Mock<IAuthenticationService> GetAuthenticationService()
        {
            var users = MockData.GetUsers();

            var user = users.FirstOrDefault();

            var userDetailsResponse = new GetUserDetailsResponse
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            var mockAuthenticationService = new Mock<IAuthenticationService>();
            mockAuthenticationService
                .Setup(repo => repo.GetUserDetailsAsync()).ReturnsAsync(userDetailsResponse);

            return mockAuthenticationService;
        }
    }
}
