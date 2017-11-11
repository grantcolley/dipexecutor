//-----------------------------------------------------------------------
// <copyright file="IBatchNotifier.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

namespace DipExecutor.Notification
{
    public interface IBatchNotifier<T>
    {
        void AddNotification(T item);
    }
}