using System.Collections.Generic;
using System.Linq;

namespace DipExecutor.Notification
{
    public class NotificationPublisher
    {
        private readonly Subscribers subscribers;

        public NotificationPublisher()
        {
            subscribers = new Subscribers();
        }

        public void Subscribe(NotificationSubscriber subscriber)
        {
            subscribers.Add(subscriber);
        }

        public void Unsubscribe(NotificationSubscriber subscriber)
        {
            subscribers.Remove(subscriber);
        }

        public void Update(List<StepNotification> notifications)
        {
            var notifyGroups = notifications.GroupBy(n => n.RunId);
            foreach (var group in notifyGroups)
            {
                var notify = subscribers.Fetch(group.Key);

            }
        }
    }
}
