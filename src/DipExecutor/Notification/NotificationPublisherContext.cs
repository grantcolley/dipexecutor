//-----------------------------------------------------------------------
// <copyright file="NotificationPublisherContext.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DipExecutor.Notification
{
    public class NotificationPublisherContext : INotificationPublisherContext
    {
        private readonly IHubContext<NotificationHub> context;

        public NotificationPublisherContext(IHubContext<NotificationHub> context)
        {
            this.context = context;
        }

        public async Task NotifyAsync(int runId, string message)
        {
            await context.Clients.Group(runId.ToString()).InvokeAsync("NotifyAsync", message);
        }
    }
}
