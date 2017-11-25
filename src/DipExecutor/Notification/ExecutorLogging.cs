//-----------------------------------------------------------------------
// <copyright file="ExecutorLogging.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

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

        public ExecutorLogging(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<ExecutorLogging>();

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