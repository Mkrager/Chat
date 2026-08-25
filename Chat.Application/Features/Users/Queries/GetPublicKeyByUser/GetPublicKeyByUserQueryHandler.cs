using Chat.Application.Contracts.Persistance;
using Chat.Application.Exceptions;
using MediatR;

namespace Chat.Application.Features.Users.Queries.GetPublicKeyByUser
{
    public class GetPublicKeyByUserQueryHandler : IRequestHandler<GetPublicKeyByUserQuery, string>
    {
        private readonly IUserRepository _userRepository;
        public GetPublicKeyByUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<string> Handle(GetPublicKeyByUserQuery request, CancellationToken cancellationToken)
        {
            var publicKey = await _userRepository.GetPublicKeyByUserId(request.UserId);

            if (string.IsNullOrEmpty(publicKey))
                throw new NotFoundException("Public key", request.UserId);

            return publicKey;
        }
    }
}
