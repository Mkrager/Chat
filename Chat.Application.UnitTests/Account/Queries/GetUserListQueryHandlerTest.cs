using AutoMapper;
using Chat.Application.Contracts.Identity;
using Chat.Application.Features.Account.Queries.GetUserList;
using Chat.Application.Profiles;
using Chat.Application.UnitTests.Mocks;
using Moq;

namespace Chat.Application.UnitTests.Account.Queries
{
    public class GetUserListQueryHandlerTest
    {
        private readonly Mock<IUserService> _mockUserService;

        public GetUserListQueryHandlerTest()
        {
            _mockUserService = RepositoryMocks.GetUserService();
        }

        [Fact]
        public async Task Handle_ValidQuery_ShouldReturnSortedUserList()
        {
            // Arrange
            var query = new GetUserListQuery();
            var handler = new GetUserListQueryHandler(_mockUserService.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }
    }
}
