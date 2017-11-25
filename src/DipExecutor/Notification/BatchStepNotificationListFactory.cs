//-----------------------------------------------------------------------
// <copyright file="BatchStepNotificationListFactory.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace DipExecutor.Notification
{
    public class BatchStepNotificationListFactory : BatchNotifierFactory<IEnumerable<StepNotification>>
    {
        private readonly INotificationPublisher notificationPublisher;
        private readonly ILoggerFactory loggerFactory;

        public BatchStepNotificationListFactory(INotificationPublisher notificationPublisher, ILoggerFactory loggerFactory)
        {
            this.notificationPublisher = notificationPublisher;
            this.loggerFactory = loggerFactory;
        }

        public override IBatchNotifier<IEnumerable<StepNotification>> GetBatchNotifier(BatchNotifierType batchNotifierType)
        {
            switch (batchNotifierType)
            {
                case BatchNotifierType.ExecutorLogging:
                    return new ExecutorLogging(loggerFactory);

                case BatchNotifierType.ExecutorPublisher:
                    return new ExecutorPublisher(notificationPublisher);
            }

            return null;
        }
    }
}
