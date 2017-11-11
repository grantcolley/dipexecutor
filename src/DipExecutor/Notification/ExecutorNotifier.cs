//-----------------------------------------------------------------------
// <copyright file="ExecutorNotifier.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using DipExecutor.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DipExecutor.Notification
{
    public class ExecutorNotifier : BatchNotifier<StepNotification>, IBatchNotifier<StepNotification>
    {
        private readonly HttpClient httpClient;

        public ExecutorNotifier(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.GetHttpClient();

            Start();
        }

        public override async Task NotifyAsync(IEnumerable<StepNotification> notifications, CancellationToken cancellationToken)
        {
            var logMessages = notifications.ToList();
            var jsonContent = JsonConvert.SerializeObject(logMessages);
            using (var response = await httpClient.PostAsync(logMessages.First<StepNotification>().NotificationUrl, new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json")))
            {
                var content = await response.Content.ReadAsStringAsync();

                // fire and forget?
            }

            using (var response = await httpClient.PostAsync(logMessages.First<StepNotification>().LoggingUrl, new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json")))
            {
                var content = await response.Content.ReadAsStringAsync();

                // fire and forget?
            }
        }
    }
}