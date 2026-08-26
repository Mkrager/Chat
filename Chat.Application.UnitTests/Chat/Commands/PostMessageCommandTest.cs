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
        private readonly Mock<IUserRepository> _mockUserRepository;
        public PostMessageCommandTest()
        {
            _mockChatRepository = ChatRepositoryMock.GetChatRepository();
            _mockUserRepository = UserServiceMock.GetUserService();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Should_Post_Messages_Successfully()
        {
            var handler = new PostMessageCommandHandler(_mapper, _mockChatRepository.Object, _mockUserRepository.Object);
            var command = new PostMessageCommand
            {
                Iv = "TestIv",
                Ciphertext = "TestCiphertext",
                ReceiverId = "d463ee4a-ad5c-4eb7-be35-3f0991bc20ad"
            };

            await handler.Handle(command, CancellationToken.None);

            var userId = Guid.Parse("35b820e5-1cea-47c8-a6a0-cedccfbda4e6");
            var receiverId = Guid.Parse("d463ee4a-ad5c-4eb7-be35-3f0991bc20ad");

            var allMessges = await _mockChatRepository.Object.ListMessages(userId, receiverId, 1, 50);
            allMessges.Count.ShouldBe(4);

            var postedMessages = allMessges.FirstOrDefault(a => a.Iv == command.Iv && a.Ciphertext == command.Ciphertext && a.CreatedBy == userId);
            postedMessages.ShouldNotBeNull();
            postedMessages.Iv.ShouldBe(command.Iv);
            postedMessages.Ciphertext.ShouldBe(command.Ciphertext);
            postedMessages.CreatedBy.ShouldBe(userId);
        }

        [Fact]
        public async void Validator_ShouldHaveError_WhenEmptyIv()
        {
            var validator = new PostMessageCommandValidator();
            var query = new PostMessageCommand
            {
                Iv = "",
                ReceiverId = "1235634645"
            };

            var result = await validator.ValidateAsync(query);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, f => f.PropertyName == "Iv");
        }

        [Fact]
        public async void Validator_ShouldHaveError_WhenIvContainsOnlySpace()
        {
            var validator = new PostMessageCommandValidator();
            var query = new PostMessageCommand
            {
                Iv = " ",
                ReceiverId = "1235634645"
            };

            var result = await validator.ValidateAsync(query);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, f => f.PropertyName == "Iv");
        }
    }
}