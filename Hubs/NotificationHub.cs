using EstateServiceApp.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstateServiceApp.Hubs
{
    public class NotificationHub:Hub
    {
        public override Task OnConnectedAsync()
        {
            ConnectedUsers.Ids.Add(Context.ConnectionId);
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            ConnectedUsers.Ids.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
        public Task SendMessage(string message)
        {
            return Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
