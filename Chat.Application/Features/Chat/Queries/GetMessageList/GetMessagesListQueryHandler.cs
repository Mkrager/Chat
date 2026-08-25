using AutoMapper;
using Chat.Application.Contracts.Persistance;
using MediatR;

namespace Chat.Application.Features.Chat.Queries.GetMessageList
{
    public class GetMessagesListQueryHandler : IRequestHandler<GetMessagesListQuery, List<MessageListVm>>
    {
        private readonly IMapper _mapper;
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        public GetMessagesListQueryHandler(
            IMapper mapper, 
            IChatRepository chatRepository, 
            IUserRepository userRepository)
        {
            _chatRepository = chatRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<List<MessageListVm>> Handle(GetMessagesListQuery request, CancellationToken cancellationToken)
        {
            var allMessages = (await _chatRepository.ListMessages(request.UserId, request.ReceiverUserId, request.Page, request.PageSize)).OrderBy(x => x.CreatedDate);

            var messagesDto = _mapper.Map<List<MessageListVm>>(allMessages);

            var userIds = allMessages
                .SelectMany(m => new[] { m.CreatedBy, m.ReceiverId })
                .Distinct()
                .ToList();

            var users = await _userRepository.GetUsersByIdsAsync(userIds);

            foreach (var message in messagesDto)
            {
                message.SenderUserName = users.First(u => u.Id == message.CreatedBy).Username;
            }

            return messagesDto;
        }
    }
}
