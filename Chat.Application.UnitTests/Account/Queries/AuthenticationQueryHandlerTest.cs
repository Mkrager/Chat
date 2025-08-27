using AutoMapper;
using Chat.Application.Contracts.Identity;
using Chat.Application.DTOs;
using Chat.Application.Features.Account.Queries.Authentication;
using Chat.Application.Profiles;
using Chat.Application.UnitTests.Mocks;
using Moq;

namespace Chat.Application.UnitTests.Account.Queries
{
    public class AuthenticationQueryHandlerTest
    {
        private readonly Mock<IAuthenticationService> _mockAuthenticationService;
        private readonly IMapper _mapper;
        public AuthenticationQueryHandlerTest()
        {
            _mockAuthenticationService = AuthenticationServiceMock.GetAuthenticationService();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_ValidCredentials_ShouldReturnAuthenticationResponse()
        {
            var handler = new AuthenticationQueryHandler(_mockAuthenticationService.Object, _mapper);

            var query = new AuthenticationQuery
            {
                Email = "test@example.com",
                Password = "TestPassword123"
            };

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.Token));
            Assert.Equal(query.Email, result.Email);

            _mockAuthenticationService.Verify(service => service.AuthenticateAsync(It.IsAny<AuthenticationRequest>()), Times.Once);
        }
    }
}