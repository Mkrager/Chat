using AutoMapper;
using Chat.Application.Contracts.Identity;
using Chat.Application.Features.Users.GetUserList;
using Chat.Application.Profiles;
using Chat.Application.UnitTests.Mocks;
using Moq;

namespace Chat.Application.UnitTests.Users
{
    public class GetUserListQueryHandlerTest
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly IMapper _mapper;
        public GetUserListQueryHandlerTest()
        {
            _mockUserService = UserServiceMock.GetUserService();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = configurationProvider.CreateMapper();

        }

        [Fact]
        public async Task Handle_ValidQuery_ShouldReturnSortedUserList()
        {
            var query = new GetUserListQuery();
            var handler = new GetUserListQueryHandler(_mockUserService.Object, _mapper);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }
    }
}
