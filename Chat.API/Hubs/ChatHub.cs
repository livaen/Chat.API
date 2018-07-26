using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.API.Hubs
{
    public class ChatHub : Hub
    {
        public readonly static ConnectionMapping<string> _connections =
            new ConnectionMapping<string>();

        [Authorize]
        public void SendPrivateMessage(string recipientUsername, string message)
        {
            var senderUsername = Context.User.Identity.Name;
            var clientConnections = _connections.GetConnections(recipientUsername);

            foreach (var clientConnection in clientConnections)
            {
                Clients.Client(clientConnection).SendAsync("sendPrivateMessage", senderUsername, message);
            }
        }

        [Authorize]
        public void SendMessageToAll(string name, string message)
        {
            Clients.All.SendAsync("sendMessageToAll", name, message);
        }

        public override Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;

            _connections.Add(name, Context.ConnectionId);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            string name = Context.User.Identity.Name;

            _connections.Remove(name, Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }


    }
}