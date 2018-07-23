using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Chat.API.Hubs
{
    public class ChatHub : Hub
    {
        public readonly static ConnectionMapping<string> _connections =
            new ConnectionMapping<string>();
        public void SendMessageToAll(string name, string message)
        {
            Clients.All.SendAsync("sendMessageToAll", name, message);
        }

      /*  public override Task OnConnectedAsync()
        {

        }*/
    }
}