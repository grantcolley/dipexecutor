//-----------------------------------------------------------------------
// <copyright file="NotificationPublisher.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using DipExecutor.Service;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DipExecutor.Notification
{
    public class NotificationPublisher : INotificationPublisher
    {
        private readonly HttpClient httpClient;
        private readonly INotificationHub notificationHub;

        public NotificationPublisher(IHttpClientFactory httpClientFactory, INotificationHub notificationHub)
        {
            httpClient = httpClientFactory.GetHttpClient();

            this.notificationHub = notificationHub;
        }

        public async Task PublishAsync(IEnumerable<StepNotification> notifications)
        {
            var notifyGroups = notifications.GroupBy(n => n.RunId);
            foreach (var group in notifyGroups)
            {
                var jsonContent = JsonConvert.SerializeObject(group);
                //notificationHub.
            }
        }
    }
}
