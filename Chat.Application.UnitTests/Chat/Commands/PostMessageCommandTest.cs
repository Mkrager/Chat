using AutoMapper;
using Chat.Application.Contracts.Identity;
using Chat.Application.Contracts.Persistance;
using Chat.Application.Features.Chat.Commands.PostMessage;
using Chat.Application.Profiles;
using Chat.Application.UnitTests.Mocks;
using Moq;
using Shouldly;

namespace Chat.Application.UnitTests.Chat.Commands
{
    public class PostMessageCommandTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IChatRepository> _mockChatRepository;
        private readonly Mock<IUserService> _mockUserService;
        public PostMessageCommandTest()
        {
            _mockChatRepository = RepositoryMocks.GetChatRepository();
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
            var handler = new PostMessageCommandHandler(_mapper, _mockChatRepository.Object, _mockUserService.Object);
            var command = new PostMessageCommand
            {
                Content = "Test4",
                ReceiverId = "d463ee4a-ad5c-4eb7-be35-3f0991bc20ad"
            };

            await handler.Handle(command, CancellationToken.None);

            string userId = "35b820e5-1cea-47c8-a6a0-cedccfbda4e6";
            string receiverId = "d463ee4a-ad5c-4eb7-be35-3f0991bc20ad";

            var allMessges = await _mockChatRepository.Object.ListAllMessages(userId, receiverId);
            allMessges.Count.ShouldBe(4);

            var postedMessages = allMessges.FirstOrDefault(a => a.Content == command.Content && a.CreatedBy == userId);
            postedMessages.ShouldNotBeNull();
            postedMessages.Content.ShouldBe(command.Content);
            postedMessages.CreatedBy.ShouldBe(userId);
        }

        [Fact]
        public async void Validator_ShouldHaveError_WhenEmptyContent()
        {
            var validator = new PostMessageCommandValidator();
            var query = new PostMessageCommand
            {
                Content = "",
                ReceiverId = "1235634645"
            };

            var result = await validator.ValidateAsync(query);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, f => f.PropertyName == "Content");
        }

        [Fact]
        public async void Validator_ShouldHaveError_WhenContentContainsOnlySpace()
        {
            var validator = new PostMessageCommandValidator();
            var query = new PostMessageCommand
            {
                Content = " ",
                ReceiverId = "1235634645"
            };

            var result = await validator.ValidateAsync(query);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, f => f.PropertyName == "Content");
        }
    }
}