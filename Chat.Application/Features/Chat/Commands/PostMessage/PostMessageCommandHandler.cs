using AutoMapper;
using Chat.Domain.Entities;
using MediatR;
using Chat.Application.Contracts.Persistance;

namespace Chat.Application.Features.Chat.Commands.PostMessage
{
    public class PostMessageCommandHandler : IRequestHandler<PostMessageCommand, PostMessageResponse>
    {
        private readonly IMapper _mapper;
        private readonly IChatRepository _chatRepository;
        private readonly IAsyncRepository<User> _userRepository;

        public PostMessageCommandHandler(
            IMapper mapper, 
            IChatRepository chatRepository, 
            IAsyncRepository<User> userRepository)
        {
            _chatRepository = chatRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<PostMessageResponse> Handle(PostMessageCommand request, CancellationToken cancellationToken)
        {
            var message = _mapper.Map<Message>(request);

            var savedMessage = await _chatRepository.AddAsync(message);
            var user = await _userRepository.GetByIdAsync(savedMessage.CreatedBy);

            var response = _mapper.Map<PostMessageResponse>(savedMessage);

            response.SenderUserName = user.Username;

            return response;
        }

    }
}
