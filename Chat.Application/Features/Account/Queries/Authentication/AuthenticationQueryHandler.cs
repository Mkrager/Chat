using AutoMapper;
using Chat.Application.Contracts.Identity;
using Chat.Application.DTOs;
using MediatR;

namespace Chat.Application.Features.Account.Queries.Authentication
{
    public class AuthenticationQueryHandler : IRequestHandler<AuthenticationQuery, AuthenticationVm>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;
        public AuthenticationQueryHandler(IAuthenticationService authenticationService, IMapper mapper)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
        }
        public async Task<AuthenticationVm> Handle(AuthenticationQuery request, CancellationToken cancellationToken)
        {
            var authenticationRequest = _mapper.Map<AuthenticationRequest>(request);

            var authenticationResult = await _authenticationService.AuthenticateAsync(authenticationRequest);

            return _mapper.Map<AuthenticationVm>(authenticationResult);
        }
    }
}
