using Chat.Application.Contracts.Identity;
using Chat.Application.DTOs;
using Moq;

namespace Chat.Application.UnitTests.Mocks
{
    public class AuthenticationServiceMock
    {
        public static Mock<IAuthenticationService> GetAuthenticationService()
        {
            var mockAuthService = new Mock<IAuthenticationService>();

            mockAuthService.Setup(service => service.RegisterAsync(It.IsAny<RegistrationRequest>()))
                .ReturnsAsync((Guid userId) => userId = Guid.NewGuid());

            mockAuthService.Setup(service => service.AuthenticateAsync(It.IsAny<AuthenticationRequest>()))
                .ReturnsAsync((AuthenticationRequest request) =>
                    new AuthenticationResponse
                    {
                        Id = Guid.NewGuid(),
                        Token = "mock-jwt-token",
                        Email = request.Email,
                        Username = "mockUser"
                    });
            return mockAuthService;
        }
    }
}