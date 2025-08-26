using AutoMapper;
using Chat.Application.Contracts.Identity;
using Chat.Application.DTOs;
using Chat.Application.Features.Account.Command.Registration;
using Chat.Application.Profiles;
using Chat.Application.UnitTests.Mocks;
using Moq;

namespace Chat.Application.UnitTests.Account.Commadns
{
    public class RegistrationCommandTest
    {
        private readonly Mock<IAuthenticationService> _mockAuthenticationService;
        private readonly IMapper _mapper;
        public RegistrationCommandTest()
        {
            _mockAuthenticationService = RepositoryMocks.GetAuthenticationService();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldReturnUserId()
        {
            var handler = new RegistrationCommandHandler(_mockAuthenticationService.Object, _mapper);

            var command = new RegistrationCommand
            {
                Email = "newuser@example.com",
                UserName = "newuser",
                Password = "NewPassword123!",
                FirstName = "New",
                LastName = "User"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result));

            _mockAuthenticationService.Verify(service => service.RegisterAsync(It.IsAny<RegistrationRequest>()), Times.Once);
        }

        [Fact]
        public async void Validator_ShouldHaveError_WhenEmptyEmail()
        {
            var validator = new RegistrationCommandValidator();
            var query = new RegistrationCommand
            {
                Email = "",
                UserName = "newuser",
                Password = "NewPassword123!",
                FirstName = "New",
                LastName = "User"
            };

            var result = await validator.ValidateAsync(query);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, f => f.PropertyName == "Email");
        }
    }
}
