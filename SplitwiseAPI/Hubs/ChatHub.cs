using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SplitwiseAPI.Hubs
{
    public class ChatHub : Hub
    {
        static Dictionary<string, string> setOfIds = new Dictionary<string, string>();

        public void AddUser(string userId)
        {
            if (!ChatHub.setOfIds.ContainsKey(userId))
                ChatHub.setOfIds.Add(userId, this.Context.ConnectionId);
            else
                ChatHub.setOfIds[userId] = this.Context.ConnectionId;

        }

        public async Task SendNotification(string type, string message,string []listOfReceivers)
        {
            foreach (var receiver in listOfReceivers)
            {
                ChatHub.setOfIds.TryGetValue(receiver, out string connectionId);
                await Clients.Client(connectionId).SendAsync("SendMessage", type, message);
            }
            
        }
    }
}
