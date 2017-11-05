using System.Collections.Generic;

namespace DipExecutor.Notification
{
    public interface INotificationPublisher
    {
        void Subscribe(Subscriber subscriber);
        void Unsubscribe(Subscriber subscriber);
        void Publish(List<StepNotification> stepNotifications);
    }
}