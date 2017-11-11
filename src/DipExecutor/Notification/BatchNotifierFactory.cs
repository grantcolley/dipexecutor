//-----------------------------------------------------------------------
// <copyright file="BatchNotifierFactory.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

namespace DipExecutor.Notification
{
    public abstract class BatchNotifierFactory<T> : IBatchNotifierFactory<T>
    {
        public abstract IBatchNotifier<T> GetBatchNotifier(BatchNotifierType batchNotifierType);
    }
}