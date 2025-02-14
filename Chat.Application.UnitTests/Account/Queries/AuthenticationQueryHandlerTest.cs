using Chat.Application.Contracts.Identity;
using Chat.Application.DTOs;
using Chat.Application.Features.Account.Queries.Authentication;
using Chat.Application.UnitTests.Mocks;
using Moq;

namespace Chat.Application.UnitTests.Account.Queries
{
    public class AuthenticationQueryHandlerTest
    {
        private readonly Mock<IAuthenticationService> _mockAuthenticationService;

        public AuthenticationQueryHandlerTest()
        {
            _mockAuthenticationService = RepositoryMocks.GetAuthenticationService();
        }

        [Fact]
        public async Task Handle_ValidCredentials_ShouldReturnAuthenticationResponse()
        {
            // Arrange
            var handler = new AuthenticationQueryHandler(_mockAuthenticationService.Object);

            var query = new AuthenticationQuery
            {
                Email = "test@example.com",
                Password = "TestPassword123"
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.Token));
            Assert.Equal(query.Email, result.Email);

            _mockAuthenticationService.Verify(service => service.AuthenticateAsync(It.IsAny<AuthenticationRequest>()), Times.Once);
        }
    }
}
