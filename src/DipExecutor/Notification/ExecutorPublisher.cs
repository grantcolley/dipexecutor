using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DipExecutor.Notification
{
    public class ExecutorPublisher : BatchNotifier<IEnumerable<StepNotification>>, IBatchNotifier<IEnumerable<StepNotification>>
    {
        private readonly INotificationPublisher notificationPublisher;

        public ExecutorPublisher(INotificationPublisher notificationPublisher)
        {
            this.notificationPublisher = notificationPublisher;

            // TODO: get this from config...
            interval = new TimeSpan(0, 0, 1);
            batchSize = null;
            queueSize = null;

            Start();
        }

        public override async Task NotifyAsync(IEnumerable<IEnumerable<StepNotification>> notifications, CancellationToken cancellationToken)
        {
            foreach(var notificationsBatch in notifications)
            {
                await notificationPublisher.PublishAsync(notificationsBatch);
            }
        }
    }
}