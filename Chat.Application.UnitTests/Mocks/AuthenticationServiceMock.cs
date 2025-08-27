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
                .ReturnsAsync((RegistrationRequest request) =>
                    new RegistrationResponse { UserId = Guid.NewGuid().ToString() });

            mockAuthService.Setup(service => service.AuthenticateAsync(It.IsAny<AuthenticationRequest>()))
                .ReturnsAsync((AuthenticationRequest request) =>
                    new AuthenticationResponse
                    {
                        Id = Guid.NewGuid().ToString(),
                        Token = "mock-jwt-token",
                        Email = request.Email,
                        UserName = "mockUser"
                    });
            return mockAuthService;
        }
    }
}