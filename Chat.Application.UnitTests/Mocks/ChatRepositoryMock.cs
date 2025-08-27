using Chat.Application.Contracts.Persistance;
using Chat.Domain.Entities;
using Moq;

namespace Chat.Application.UnitTests.Mocks
{
    public class ChatRepositoryMock
    {
        public static Mock<IChatRepository> GetChatRepository()
        {
            var messages = new List<Message>
            {
                new Message
                {
                    Id = Guid.Parse("d13b166b-8f28-4614-9fb7-32322fee1765"),
                    Content = "Test",
                    CreatedDate = DateTime.Now.AddDays(1),
                    CreatedBy = "35b820e5-1cea-47c8-a6a0-cedccfbda4e6",
                    ReceiverId = "d463ee4a-ad5c-4eb7-be35-3f0991bc20ad"
                },
                new Message
                {
                    Id = Guid.Parse("d13b166b-8f28-4614-9fb7-32322fee1765"),
                    Content = "Test2",
                    CreatedDate = DateTime.Now.AddDays(1),
                    CreatedBy = "35b820e5-1cea-47c8-a6a0-cedccfbda4e6",
                    ReceiverId = "d463ee4a-ad5c-4eb7-be35-3f0991bc20ad"
                },
                new Message
                {
                    Id = Guid.Parse("d13b166b-8f28-4614-9fb7-32322fee1765"),
                    Content = "Test3",
                    CreatedDate = DateTime.Now.AddDays(1),
                    CreatedBy = "35b820e5-1cea-47c8-a6a0-cedccfbda4e6",
                    ReceiverId = "d463ee4a-ad5c-4eb7-be35-3f0991bc20ad"
                },
            };

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
    }
}