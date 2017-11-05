using System.Collections.Generic;
using System.Linq;

namespace DipExecutor.Notification
{
    public class NotificationPublisher : INotificationPublisher
    {
        private readonly Subscribers subscribers;
        private readonly IBatchNotifier<PublishNotifications> batchNotifier;

        public NotificationPublisher(IBatchNotifier<PublishNotifications> batchNotifier)
        {
            subscribers = new Subscribers();
            this.batchNotifier = batchNotifier;
        }

        public void Subscribe(Subscriber subscriber)
        {
            subscribers.Add(subscriber);
        }

        public void Unsubscribe(Subscriber subscriber)
        {
            subscribers.Remove(subscriber);
        }

        public void Publish(List<StepNotification> stepNotifications)
        {
            var notifyGroups = stepNotifications.GroupBy(n => n.RunId);
            foreach (var group in notifyGroups)
            {
                var urls = subscribers.FetchUrls(group.Key);
                if (urls.Any())
                {
                    var notifications = new PublishNotifications { Urls = urls, StepNotifications = group };
                    batchNotifier.AddNotification(notifications);
                }
            }
        }
    }
}
