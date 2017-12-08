using DipExecutor.Notification;
using DipRunner;
using System.Collections.Generic;

namespace DipExecutor.Test
{
    public class BatchNotifierTest : IBatchNotifier<StepNotification>
    {
        public void AddNotification(StepNotification notification)
        {
            // do nothing...
        }

        public void AddNotifications(List<StepNotification> notifications)
        {
            // do nothing...
        }
    }
}
