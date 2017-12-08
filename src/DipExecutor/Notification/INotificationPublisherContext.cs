//-----------------------------------------------------------------------
// <copyright file="INotificationPublisherContext.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using DipRunner;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DipExecutor.Notification
{
    public interface INotificationPublisherContext
    {
        Task NotifyAsync(int runId, IEnumerable<StepNotification> message);
    }
}