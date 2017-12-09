//-----------------------------------------------------------------------
// <copyright file="ExecutorHelper.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using DipRunner;
using System;

namespace DipExecutor.Utilities
{
    internal static class ExecutorHelper
    {
        internal static StepNotification CreateStepNotification(this Step step, NotificationLevel notificationLevel, int notificationEventId, string message)
        {
            var stepNotification = new StepNotification
            {
                RunId = step.RunId,
                RunName = step.RunName,
                StepId = step.StepId,
                StepName = step.StepName,
                Status = step.Status,
                Message = message,
                NotificationLevel = notificationLevel,
                StepUrl = step.StepUrl,
                NotificationUrl = step.NotificationUrl,
                LogUrl = step.LogUrl,
                Machine = Environment.MachineName,
                Timestamp = DateTimeOffset.Now,
                NotificationEventId = notificationEventId
            };

            return stepNotification;
        }
    }
}
