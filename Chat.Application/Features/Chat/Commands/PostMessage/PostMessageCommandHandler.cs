using AutoMapper;
using Chat.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Chat.Application.ChatHub;
using Chat.Application.Contracts.Persistance;
using Chat.Application.Contracts.Identity;

namespace Chat.Application.Features.Chat.Commands.PostMessage
{
    public class PostMessageCommandHandler : IRequestHandler<PostMessageCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly IChatRepository _chatRepository;
        private readonly IHubContext<ChatHubs> _hubContext;
        private readonly IUserService _userService;

        public PostMessageCommandHandler(IMapper mapper, IChatRepository chatRepository, IHubContext<ChatHubs> hubContext, IUserService userService)
        {
            _chatRepository = chatRepository;
            _mapper = mapper;
            _hubContext = hubContext;
            _userService = userService;
        }

        public async Task<Guid> Handle(PostMessageCommand request, CancellationToken cancellationToken)
        {
            var validator = new PostMessageCommandValidator();
            var validatorResult = await validator.ValidateAsync(request);

            if (validatorResult.Errors.Count > 0)
                throw new Exceptions.ValidationException(validatorResult);

            var userDetails = await _userService.GetUserDetailsAsync();
            var userId = userDetails.UserId;
            var senderUserName = userDetails.UserName;

            var message = new Message
            {
                Content = request.Content,
                SendDate = request.SendDate,
                UserId = userId,
                SenderUserName = senderUserName,
                ReceiverUserId = request.ReceiverUserId
            };

            _mapper.Map(request, message);

            message = await _chatRepository.PostMessage(message);

            await _hubContext.Clients.Group(request.ReceiverUserId).SendAsync("SendMessage", userDetails.UserName, message.Content);

            return @message.Id;
        }
    }
}
