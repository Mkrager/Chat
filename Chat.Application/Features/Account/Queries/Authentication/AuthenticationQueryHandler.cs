using Chat.Application.Contracts.Identity;
using Chat.Application.DTOs;
using MediatR;

namespace Chat.Application.Features.Account.Queries.Authentication
{
    public class AuthenticationQueryHandler : IRequestHandler<AuthenticationQuery, AuthenticationResponse>
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationQueryHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        public async Task<AuthenticationResponse> Handle(AuthenticationQuery request, CancellationToken cancellationToken)
        {
            var authenticationRequest = new AuthenticationRequest
            {
                Email = request.Email,
                Password = request.Password
            };

            var authenticationResult = await _authenticationService.AuthenticateAsync(authenticationRequest);

            return authenticationResult;
        }
    }
}
