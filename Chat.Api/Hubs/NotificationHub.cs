using Chat.Application.Contracts;
using Chat.Application.Features.Chat.Commands.PostMessage;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Api.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;
        public NotificationHub(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }
        public async Task JoinGroup(string recieverId)
        {
            var currentUserId = _currentUserService.UserId;

            var groupName = GetGroupName(currentUserId, recieverId);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.SendAsync("GroupJoined", recieverId);
        }

        public async Task LeaveGroup(string recieverId)
        {
            var currentUserId = _currentUserService.UserId;

            var groupName = GetGroupName(currentUserId, recieverId);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.SendAsync("GroupLeft", groupName);
        }

        public async Task SendMessageToGroup(string recieverId, string message, string receiverUserId)
        {
            var currentUserId = _currentUserService.UserId;

            var groupName = GetGroupName(currentUserId, recieverId);

            var result = await _mediator.Send(new PostMessageCommand()
            {
                Content = message,
                ReceiverId = receiverUserId
            });

            await Clients.Group(groupName).SendAsync("SendMessage", result);
        }

        private string GetGroupName(string user1Id, string user2Id)
        {
            var ordered = new[] { user1Id, user2Id }.OrderBy(x => x).ToArray();
            return $"chat_{ordered[0]}_{ordered[1]}";
        }
    }
}
