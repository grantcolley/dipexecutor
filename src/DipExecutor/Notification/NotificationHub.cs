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
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var runId = Context.Connection.GetHttpContext().Request.Query["runid"];
            await Groups.AddAsync(Context.ConnectionId, runId);
            await Clients.Client(Context.ConnectionId).InvokeAsync("Connected", $"Connected and listening for notifications from Run Id {runId}");
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}