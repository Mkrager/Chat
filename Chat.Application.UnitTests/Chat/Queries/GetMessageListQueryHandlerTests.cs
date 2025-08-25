using AutoMapper;
using Chat.Application.Contracts.Identity;
using Chat.Application.Contracts.Persistance;
using Chat.Application.Features.Chat.Queries.GetMessageList;
using Chat.Application.Profiles;
using Chat.Application.UnitTests.Mocks;
using Moq;
using Shouldly;

namespace Chat.Application.UnitTests.Chat.Queries
{
    public class GetMessageListQueryHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IChatRepository> _mockChatRepository;
        private readonly Mock<IUserService> _mockUserService;

        public GetMessageListQueryHandlerTest()
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
        public async Task GetMessagesList_ShouldReturnListOfMessages_WhenInvoked()
        {
            var handler = new GetMessageListQueryHandler(_mapper, _mockChatRepository.Object, _mockUserService.Object);

            string userId = "35b820e5-1cea-47c8-a6a0-cedccfbda4e6";
            string ReceiverUserId = "d463ee4a-ad5c-4eb7-be35-3f0991bc20ad";

            var result = await handler.Handle(new GetMessageListQuery() 
            { 
                UserId = userId, 
                ReceiverUserId = ReceiverUserId 
            }, CancellationToken.None);

            result.ShouldBeOfType<List<MessageListVm>>();

            result.Count.ShouldBe(3);
        }
    }
}
