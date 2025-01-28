using AutoMapper;
using Chat.Domain.Interfaces.Identity;
using Chat.Domain.Models;
using MediatR;

namespace Chat.Application.Features.Account.Queries.Registration
{
    public class RegistrationQueryHandler : IRequestHandler<RegistrationQuery, RegistrationResponse>
    {
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;

        public RegistrationQueryHandler(IMapper mapper, IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        public async Task<RegistrationResponse> Handle(RegistrationQuery request, CancellationToken cancellationToken)
       {
            var validator = new RegistrationQueryValidator();
            var validatorResult = await validator.ValidateAsync(request);

            if (validatorResult.Errors.Count > 0)
                throw new Exceptions.ValidationException(validatorResult);

            var registerUser = new RegistrationRequest
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password,
                UserName = request.UserName
            };

            var register = await _authenticationService.RegisterAsync(registerUser);

            var registrationResponse = _mapper.Map<RegistrationResponse>(register);

            return registrationResponse;
        }
    }
}
