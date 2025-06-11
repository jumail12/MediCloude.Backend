using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;


namespace BusinessService.Infrastructure.VideoCallSetup
{
    public class VideoHub : Hub
    {
        private static ConcurrentDictionary<string, string> userRooms = new ConcurrentDictionary<string, string>();

        public async Task JoinRoom(string roomName)
        {
            userRooms[Context.ConnectionId] = roomName;
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task LeaveRoom(string roomName)
        {
            if (userRooms.TryRemove(Context.ConnectionId, out _))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
                await Clients.OthersInGroup(roomName).SendAsync("ForceEndCall");
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (userRooms.TryRemove(Context.ConnectionId, out string roomName))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
                await Clients.OthersInGroup(roomName).SendAsync("ForceEndCall");
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task CallUser(string roomName)
        {
            await Clients.OthersInGroup(roomName).SendAsync("IncomingCall", Context.ConnectionId);
        }

        public async Task AcceptCall(string roomName)
        {
            await Clients.OthersInGroup(roomName).SendAsync("CallAccepted", Context.ConnectionId);
        }

        public async Task RejectCall(string roomName)
        {
            await Clients.OthersInGroup(roomName).SendAsync("CallRejected", Context.ConnectionId);
        }

        public async Task SendOffer(string roomName, string offer)
        {
            await Clients.OthersInGroup(roomName).SendAsync("ReceiveOffer", offer);
        }

        public async Task SendAnswer(string roomName, string answer)
        {
            await Clients.OthersInGroup(roomName).SendAsync("ReceiveAnswer", answer);
        }

        public async Task SendIceCandidate(string roomName, string candidate)
        {
            await Clients.OthersInGroup(roomName).SendAsync("ReceiveIceCandidate", candidate);
        }
    }
}
