using AutoMapper;
using Chat.Application.Contracts.Identity;
using Chat.Application.Contracts.Persistance;
using MediatR;

namespace Chat.Application.Features.Chat.Queries.GetMessageList
{
    public class GetMessageListQueryHandler : IRequestHandler<GetMessageListQuery, List<MessageListVm>>
    {
        private readonly IMapper _mapper;
        private readonly IChatRepository _chatRepository;
        private readonly IUserService _userService;
        public GetMessageListQueryHandler(IMapper mapper, IChatRepository chatRepository, IUserService userService)
        {
            _chatRepository = chatRepository;
            _mapper = mapper;
            _userService = userService;
        }
        public async Task<List<MessageListVm>> Handle(GetMessageListQuery request, CancellationToken cancellationToken)
        {
            var allMessages = (await _chatRepository.ListAllMessages(request.UserId, request.ReceiverUserId)).OrderBy(x => x.CreatedDate);

            var messagesDto = _mapper.Map<List<MessageListVm>>(allMessages);

            var userIds = allMessages
                .SelectMany(m => new[] { m.CreatedBy, m.ReceiverId })
                .Distinct()
                .ToList();

            var users = await _userService.GetUsersByIdsAsync(userIds);

            foreach (var message in messagesDto)
            {
                message.SenderUserName = users.First(u => u.UserId == message.CreatedBy).UserName;
                message.ReceiverUserName = users.First(u => u.UserId == message.ReceiverId).UserName;
            }

            return messagesDto;
        }
    }
}
