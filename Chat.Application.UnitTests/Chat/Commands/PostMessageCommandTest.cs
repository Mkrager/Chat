using AutoMapper;
using Chat.Application.ChatHub;
using Chat.Application.Contracts.Identity;
using Chat.Application.Contracts.Persistance;
using Chat.Application.Features.Chat.Commands.PostMessage;
using Chat.Application.Profiles;
using Chat.Application.UnitTests.Mocks;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Shouldly;

namespace Chat.Application.UnitTests.Chat.Commands
{
    public class PostMessageCommandTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IChatRepository> _mockChatRepository;
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IHubContext<ChatHubs>> _mockHubContext;

        public PostMessageCommandTest()
        {
            _mockChatRepository = RepositoryMocks.GetChatRepository();
            _mockHubContext =  RepositoryMocks.GetHubContext();
            _mockUserService = RepositoryMocks.GetUserService();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Should_Post_Messages_Successfully()
        {
            // Arrange
            var handler = new PostMessageCommandHandler(_mapper, _mockChatRepository.Object, _mockHubContext.Object, _mockUserService.Object);
            var command = new PostMessageCommand
            {
                Content = "Test",
                SendDate = DateTime.UtcNow,
                ReceiverUserId = "d463ee4a-ad5c-4eb7-be35-3f0991bc20ad"
            };

            // Act
            await handler.Handle(command, CancellationToken.None);

            string userId = "35b820e5-1cea-47c8-a6a0-cedccfbda4e6";
            string receiverId = "d463ee4a-ad5c-4eb7-be35-3f0991bc20ad";

            // Assert
            var allMessges = await _mockChatRepository.Object.ListAllMessages(userId, receiverId);
            allMessges.Count.ShouldBe(4);

            var postedMessages = allMessges.FirstOrDefault(a => a.Content == command.Content && a.UserId == userId);
            postedMessages.ShouldNotBeNull();
            postedMessages.Content.ShouldBe(command.Content);
            postedMessages.UserId.ShouldBe(userId);
            postedMessages.SendDate.ShouldBe(command.SendDate);
        }

    }
}
