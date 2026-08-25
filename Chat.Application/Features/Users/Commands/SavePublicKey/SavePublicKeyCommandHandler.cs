using Chat.Application.Contracts.Persistance;
using Chat.Application.Exceptions;
using Chat.Domain.Entities;
using MediatR;

namespace Chat.Application.Features.Users.Commands.SavePublicKey
{
    public class SavePublicKeyCommandHandler : IRequestHandler<SavePublicKeyCommand>
    {
        private readonly IUserRepository _userRepository;
        public SavePublicKeyCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Unit> Handle(SavePublicKeyCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user == null)
                throw new NotFoundException(nameof(User), request.UserId);

            if (user.PublicKey != null)
                return Unit.Value;

            await _userRepository.SavePublicKey(user, request.PublicKey);

            return Unit.Value;
        }
    }
}
