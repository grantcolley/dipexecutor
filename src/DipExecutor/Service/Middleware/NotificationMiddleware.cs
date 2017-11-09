using DipExecutor.Notification;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DipExecutor.Service.Middleware
{
    public class NotificationMiddleware
    {
        private readonly INotificationPublisher notificationPublisher;

        public NotificationMiddleware(RequestDelegate next, INotificationPublisher notificationPublisher)
        {
            this.notificationPublisher = notificationPublisher;
        }

        public async Task Invoke(HttpContext context)
        {
            string body;
            var stream = context.Request.Body;
            using (var reader = new StreamReader(stream))
            {
                body = await reader.ReadToEndAsync();
            }

            var stepNotifications = JsonConvert.DeserializeObject<List<StepNotification>>(body);
            
            await notificationPublisher.PublishAsync(stepNotifications);
        }
    }
}