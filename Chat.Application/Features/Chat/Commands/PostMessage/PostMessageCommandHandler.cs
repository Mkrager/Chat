using AutoMapper;
using Chat.Domain.Entities;
using MediatR;
using Chat.Application.Contracts.Persistance;
using Chat.Application.Contracts.Identity;

namespace Chat.Application.Features.Chat.Commands.PostMessage
{
    public class PostMessageCommandHandler : IRequestHandler<PostMessageCommand, PostMessageResponse>
    {
        private readonly IMapper _mapper;
        private readonly IChatRepository _chatRepository;
        private readonly IUserService _userService;

        public PostMessageCommandHandler(IMapper mapper, IChatRepository chatRepository, IUserService userService)
        {
            _chatRepository = chatRepository;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<PostMessageResponse> Handle(PostMessageCommand request, CancellationToken cancellationToken)
        {
            var message = _mapper.Map<Message>(request);

            var savedMessage = await _chatRepository.AddAsync(message);
            var user = await _userService.GetUserDetailsAsync(savedMessage.CreatedBy);

            var response = _mapper.Map<PostMessageResponse>(savedMessage);
            response.SenderUserName = user.UserName;

            return response;
        }

    }
}
