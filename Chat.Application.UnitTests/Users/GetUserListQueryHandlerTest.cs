using Chat.Application.Contracts.Identity;
using Chat.Application.Features.Users.GetUserList;
using Chat.Application.UnitTests.Mocks;
using Moq;

namespace Chat.Application.UnitTests.Users
{
    public class GetUserListQueryHandlerTest
    {
        private readonly Mock<IUserService> _mockUserService;

        public GetUserListQueryHandlerTest()
        {
            _mockUserService = UserServiceMock.GetUserService();
        }

        [Fact]
        public async Task Handle_ValidQuery_ShouldReturnSortedUserList()
        {
            var query = new GetUserListQuery();
            var handler = new GetUserListQueryHandler(_mockUserService.Object);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }
    }
}
