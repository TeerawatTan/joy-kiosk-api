using Microsoft.AspNetCore.SignalR;

namespace JoyKioskApi.Hubs
{
    public class MessageHub : Hub<IMessageHubClient>
    {
        public async Task ResultPayment(string message)
        {
            await Clients.All.ResultPayment(message);
        }
    }
}
