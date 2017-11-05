using DipExecutor.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DipExecutor.Service.Middleware
{
    public class NotificationMiddleware
    {
        ILogger logger;
        INotificationPublisher notificationPublisher;

        public NotificationMiddleware(RequestDelegate next, INotificationPublisher notificationPublisher, ILoggerFactory logger)
        {
            this.notificationPublisher = notificationPublisher;
            this.logger = logger.CreateLogger(typeof(NotificationMiddleware));
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

            //logger.Log<StepNotification>(GetStepNotificationLogLevel(stepNotification), stepNotification.NotificationEventId, stepNotification, null, null);

            notificationPublisher.Publish(stepNotifications);
        }

        private LogLevel GetStepNotificationLogLevel(StepNotification stepNotification)
        {
            switch(stepNotification.NotificationLevel)
            {
                case NotificationLevel.Debug:
                    return LogLevel.Debug;
                case NotificationLevel.Information:
                    return LogLevel.Information;
                case NotificationLevel.Warning:
                    return LogLevel.Warning;
                case NotificationLevel.Error:
                    return LogLevel.Error;
            }

            return LogLevel.Information;
        }
    }
}
