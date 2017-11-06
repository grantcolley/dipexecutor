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
        private readonly Subscribers subscribers;
        private readonly HttpClient httpClient;

        public NotificationPublisher(IHttpClientFactory httpClientFactory)
        {
            subscribers = new Subscribers();
            httpClient = httpClientFactory.GetHttpClient();
        }

        public void Subscribe(Subscriber subscriber)
        {
            subscribers.Add(subscriber);
        }

        public void Unsubscribe(Subscriber subscriber)
        {
            subscribers.Remove(subscriber);
        }

        public async Task PublishAsync(IEnumerable<StepNotification> notifications)
        {
            var notifyGroups = notifications.GroupBy(n => n.RunId);
            foreach (var group in notifyGroups)
            {
                var urls = subscribers.FetchUrls(group.Key);
                if (urls.Any())
                {
                    var jsonContent = JsonConvert.SerializeObject(group);
                    
                    foreach (var url in urls)
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
}
