using System.Collections.Generic;

namespace DipExecutor.Notification
{
    public class PublishNotifications
    {
        public IEnumerable<string> Urls { get; set; }
        public IEnumerable<StepNotification> StepNotifications { get; set; }
    }
}