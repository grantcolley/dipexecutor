using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DipExecutor.Notification
{
    public class ExecutorLogging : BatchNotifier<IEnumerable<StepNotification>>, IBatchNotifier<IEnumerable<StepNotification>>
    {
        private readonly ILogger logger;

        public ExecutorLogging(ILoggerFactory logger)
        {
            this.logger = logger.CreateLogger(typeof(ExecutorLogging));

            // TODO: get this from config...
            interval = new TimeSpan(0, 0, 1);
            batchSize = null;
            queueSize = null;

            Start();
        }

        public override async Task NotifyAsync(IEnumerable<IEnumerable<StepNotification>> notifications, CancellationToken cancellationToken)
        {
            var flattenedList = notifications.SelectMany(n => n);
            foreach (var stepNotification in flattenedList)
            {
                logger.Log<StepNotification>(GetStepNotificationLogLevel(stepNotification), stepNotification.NotificationEventId, stepNotification, null, null);
            }
        }

        private LogLevel GetStepNotificationLogLevel(StepNotification stepNotification)
        {
            switch (stepNotification.NotificationLevel)
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