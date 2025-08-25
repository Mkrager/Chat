using Chat.Application.Features.Chat.Commands.PostMessage;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Api.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly IMediator _mediator;

        public NotificationHub(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.SendAsync("GroupJoined", groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.SendAsync("GroupLeft", groupName);
        }

        public async Task SendMessageToGroup(string groupName, string message, string receiverUserId)
        {
            var result = await _mediator.Send(new PostMessageCommand()
            {
                Content = message,
                ReceiverId = receiverUserId
            });

            await Clients.Group(groupName).SendAsync("SendMessage", result);
        }
    }
}
