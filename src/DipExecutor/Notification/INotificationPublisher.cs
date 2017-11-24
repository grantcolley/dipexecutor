//-----------------------------------------------------------------------
// <copyright file="INotificationPublisher.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace DipExecutor.Notification
{
    public interface INotificationPublisher
    {
        Task PublishAsync(IEnumerable<StepNotification> notifications);
    }
}