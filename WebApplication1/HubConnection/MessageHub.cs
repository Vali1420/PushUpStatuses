using Microsoft.AspNetCore.SignalR;

namespace WebApplication1.HubConnection
{
    public class MessageHub: Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessageHandler", message);
        }
    }
}
