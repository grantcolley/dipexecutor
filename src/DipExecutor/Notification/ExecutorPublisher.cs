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
    public class ExecutorPublisher : BatchNotifier<PublishNotifications>
    {
        private readonly HttpClient httpClient;

        public ExecutorPublisher(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.GetHttpClient();

            // TODO: get this from config...
            interval = new TimeSpan(0, 0, 1);
            batchSize = null;
            queueSize = null;

            Start();
        }

        public override async Task NotifyAsync(IEnumerable<PublishNotifications> notifications, CancellationToken cancellationToken)
        {
            foreach(var notification in notifications)
            {
                var jsonContent = JsonConvert.SerializeObject(notification.StepNotifications);

                foreach (var url in notification.Urls)
                {
                    using (var response = await httpClient.PostAsync(url, new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json")))
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        // fire and forget?
                    }
                }
            }
        }
    }
}