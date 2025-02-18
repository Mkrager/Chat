using Chat.Application.Contracts.Identity;
using Chat.Application.DTOs;
using Chat.Application.Features.Account.Command.Registration;
using Chat.Application.UnitTests.Mocks;
using Moq;

namespace Chat.Application.UnitTests.Account.Commadns
{
    public class RegistrationCommandTest
    {
        private readonly Mock<IAuthenticationService> _mockAuthenticationService;

        public RegistrationCommandTest()
        {
            _mockAuthenticationService = RepositoryMocks.GetAuthenticationService();
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldReturnUserId()
        {
            // Arrange
            var handler = new RegistrationCommandHandler(_mockAuthenticationService.Object);

            var command = new RegistrationCommand
            {
                Email = "newuser@example.com",
                UserName = "newuser",
                Password = "NewPassword123!",
                FirstName = "New",
                LastName = "User"
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.UserId));

            _mockAuthenticationService.Verify(service => service.RegisterAsync(It.IsAny<RegistrationRequest>()), Times.Once);
        }
    }
}
