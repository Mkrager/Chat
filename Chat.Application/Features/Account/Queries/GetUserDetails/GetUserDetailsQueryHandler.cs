using AutoMapper;
using Chat.Domain.Interfaces.Identity;
using Chat.Domain.Models;
using MediatR;

namespace Chat.Application.Features.Account.Queries.GetUserDetails
{
    public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, GetUserDetailsResponse>
    {
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;

        public GetUserDetailsQueryHandler(IMapper mapper, IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
        }
        public async Task<GetUserDetailsResponse> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
        {
            var user = await _authenticationService.GetUserDetailsAsync();
            return _mapper.Map<GetUserDetailsResponse>(user);
        }

    }
}
