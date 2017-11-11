//-----------------------------------------------------------------------
// <copyright file="BatchStepNotificationListFactory.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using System.Collections.Generic;

namespace DipExecutor.Notification
{
    public class BatchStepNotificationListFactory : BatchNotifierFactory<IEnumerable<StepNotification>>
    {
        private readonly INotificationPublisher notificationPublisher;

        public BatchStepNotificationListFactory(INotificationPublisher notificationPublisher)
        {
            this.notificationPublisher = notificationPublisher;
        }

        public override IBatchNotifier<IEnumerable<StepNotification>> GetBatchNotifier(BatchNotifierType batchNotifierType)
        {
            switch (batchNotifierType)
            {
                case BatchNotifierType.ExecutorLogging:
                    return new ExecutorLogging(null);

                case BatchNotifierType.ExecutorPublisher:
                    return new ExecutorPublisher(notificationPublisher);
            }

            return null;
        }
    }
}
