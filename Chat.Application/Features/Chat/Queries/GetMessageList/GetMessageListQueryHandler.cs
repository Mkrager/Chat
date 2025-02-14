using AutoMapper;
using Chat.Application.Contracts.Persistance;
using MediatR;

namespace Chat.Application.Features.Chat.Queries.GetMessageList
{
    public class GetMessageListQueryHandler : IRequestHandler<GetMessageListQuery, List<MessageListVm>>
    {
        private readonly IMapper _mapper;
        private readonly IChatRepository _chatRepository;
        public GetMessageListQueryHandler(IMapper mapper, IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
            _mapper = mapper;
        }
        public async Task<List<MessageListVm>> Handle(GetMessageListQuery request, CancellationToken cancellationToken)
        {
            var allMessage = (await _chatRepository.ListAllMessages(request.UserId, request.ReceiverUserId)).OrderBy(x => x.SendDate);
            var messagesDto = _mapper.Map<List<MessageListVm>>(allMessage);

            return messagesDto;
        }
    }
}
