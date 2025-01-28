using AutoMapper;
using Chat.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Chat.Application.ChatHub;
using Chat.Domain.Interfaces.Repositories;
using Chat.Domain.Interfaces.Identity;

namespace Chat.Application.Features.Chat.Commands.PostMessage
{
    public class PostMessageCommandHandler : IRequestHandler<PostMessageCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly IChatRepository _chatRepository;
        private readonly IHubContext<ChatHubs> _hubContext;
        private readonly IAuthenticationService _authenticationService;

        public PostMessageCommandHandler(IMapper mapper, IChatRepository chatRepository, IHubContext<ChatHubs> hubContext, IAuthenticationService authenticationService)
        {
            _chatRepository = chatRepository;
            _mapper = mapper;
            _hubContext = hubContext;
            _authenticationService = authenticationService;
        }

        public async Task<Guid> Handle(PostMessageCommand request, CancellationToken cancellationToken)
        {
            var validator = new PostMessageCommandValidator();
            var validatorResult = await validator.ValidateAsync(request);

            if (validatorResult.Errors.Count > 0)
                throw new Exceptions.ValidationException(validatorResult);

            var userDetails = await _authenticationService.GetUserDetailsAsync();
            var userId = userDetails.UserId;

            var message = new Message
            {
                Content = request.Content,
                SendDate = request.SendDate,
                UserId = userId,
                ReceiverUserId = request.ReceiverUserId
            };

            _mapper.Map(request, message);

            message = await _chatRepository.PostMessage(message);

            await _hubContext.Clients.Group(request.ReceiverUserId).SendAsync("SendMessage", userDetails.UserName, message.Content);

            return @message.Id;
        }
    }
}
