//-----------------------------------------------------------------------
// <copyright file="INotificationHub.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using System.Threading.Tasks;

namespace DipExecutor.Notification
{
    public interface INotificationHub
    {
        Task SendAsync(string connectionId, string method, string message);
    }
}