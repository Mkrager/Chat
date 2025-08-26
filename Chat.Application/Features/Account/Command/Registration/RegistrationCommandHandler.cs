using Chat.Application.Contracts.Identity;
using Chat.Application.DTOs;
using MediatR;

namespace Chat.Application.Features.Account.Command.Registration
{
    public class RegistrationCommandHandler : IRequestHandler<RegistrationCommand, RegistrationResponse>
    {
        private readonly IAuthenticationService _authenticationService;

        public RegistrationCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<RegistrationResponse> Handle(RegistrationCommand request, CancellationToken cancellationToken)
        {
            var registerUser = new RegistrationRequest
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password,
                UserName = request.UserName
            };

            var register = await _authenticationService.RegisterAsync(registerUser);

            return register;
        }
    }
}
