//-----------------------------------------------------------------------
// <copyright file="BatchStepNotificationFactory.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using DipExecutor.Service;

namespace DipExecutor.Notification
{
    public class BatchStepNotificationFactory : BatchNotifierFactory<StepNotification>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public BatchStepNotificationFactory(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public override IBatchNotifier<StepNotification> GetBatchNotifier(BatchNotifierType batchNotifierType)
        {
            switch (batchNotifierType)
            {
                case BatchNotifierType.ExecutorNotifier:
                    return new ExecutorNotifier(httpClientFactory);
            }

            return null;
        }
    }
}