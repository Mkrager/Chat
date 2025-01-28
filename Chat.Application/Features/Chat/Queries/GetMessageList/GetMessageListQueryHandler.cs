using AutoMapper;
using Chat.Domain.Interfaces.Repositories;
using MediatR;

namespace Chat.Application.Features.Chat.Queries.GetMessageList
{
    public class GetMessageListQueryHandler : IRequestHandler<GetMessageListQuery, List<MessageListVm>>
    {
        private readonly IMapper _mapper;
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        public GetMessageListQueryHandler(IMapper mapper, IChatRepository chatRepository, IUserRepository userRepository)
        {
            _chatRepository = chatRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<List<MessageListVm>> Handle(GetMessageListQuery request, CancellationToken cancellationToken)
        {
            var allMessage = (await _chatRepository.ListAllMessages(request.UserId, request.ReceiverUserId)).OrderBy(x => x.SendDate);
            var messagesDto = _mapper.Map<List<MessageListVm>>(allMessage);

            var users = await _userRepository.ListAllUsers();
            foreach (var message in messagesDto)
            {
                message.User = _mapper.Map<MessageUserDto>(users.FirstOrDefault(c => c.UserId == message.UserId));
            }
            return messagesDto;
        }
    }
}
