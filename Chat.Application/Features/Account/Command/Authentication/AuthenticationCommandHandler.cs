using AutoMapper;
using Chat.Domain.Interfaces.Identity;
using Chat.Domain.Models;
using MediatR;

namespace Chat.Application.Features.Account.Command.Authentication
{
    public class AuthenticationCommandHandler : IRequestHandler<AuthenticationCommand, AuthenticationResponse>
    {
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationCommandHandler(IMapper mapper, IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
        }
        public async Task<AuthenticationResponse> Handle(AuthenticationCommand request, CancellationToken cancellationToken)
        {
            var validator = new AuthenticationQueryCommandValidator();
            var validatorResult = await validator.ValidateAsync(request);

            if (validatorResult.Errors.Count > 0)
                throw new Exceptions.ValidationException(validatorResult);


            var authenticationRequest = new AuthenticationRequest
            {
                Email = request.Email,
                Password = request.Password
            };

            var authenticationResult = await _authenticationService.AuthenticateAsync(authenticationRequest);

            var authenticationResponse = _mapper.Map<AuthenticationResponse>(authenticationResult);

            return authenticationResponse;
        }
    }
}
