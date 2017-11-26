//-----------------------------------------------------------------------
// <copyright file="NotificationHub.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace DipExecutor.Notification
{
    public class NotificationHub : Hub, INotificationHub
    {
        private readonly Subscribers subscribers;

        public NotificationHub()
        {
            subscribers = new Subscribers();
        }

        public async Task NotifyAsync(int runId, string message)
        {
            var notifySubscribers = subscribers.GetByRunId(runId.ToString());
            foreach (var subscriber in notifySubscribers)
            {
                await SendAsync(subscriber.ConnectionId, "sendasync", message);
            }
        }

        public Task SendAsync(string connectionId, string method, string message)
        {
            return Clients.Client(connectionId).InvokeAsync(method, message);
        }
        
        public override Task OnConnectedAsync()
        {
            var runId = Context.Connection.GetHttpContext().Request.Query["runid"];
            Subscribe(Context.ConnectionId, runId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Unsubscribe(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        private void Subscribe(string connectionId, string runId)
        {
            subscribers.Add(connectionId, runId);
        }

        public void Unsubscribe(string connectionId)
        {
            subscribers.RemoveByConnectionId(connectionId);
        }
    }
}