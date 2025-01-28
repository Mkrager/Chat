using Microsoft.AspNetCore.SignalR;

namespace Chat.Application.ChatHub
{
    public class ChatHubs : Hub
    {
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

        public async Task SendMessageToGroup(string groupName, string senderName, string message)
        {
            await Clients.Group(groupName).SendAsync("SendMessage", senderName, message);
        }
    }
}
