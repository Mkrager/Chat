using AutoMapper;
using Chat.Application.Contracts.Identity;
using Chat.Application.DTOs;
using MediatR;

namespace Chat.Application.Features.Account.Command.Registration
{
    public class RegistrationCommandHandler : IRequestHandler<RegistrationCommand, string>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;
        public RegistrationCommandHandler(IAuthenticationService authenticationService, IMapper mapper)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        public async Task<string> Handle(RegistrationCommand request, CancellationToken cancellationToken)
        {
            var registerUser = _mapper.Map<RegistrationRequest>(request);
                
            var register = await _authenticationService.RegisterAsync(registerUser);

            return register.UserId;
        }
    }
}
