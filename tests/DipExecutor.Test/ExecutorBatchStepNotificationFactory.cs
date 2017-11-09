using DipExecutor.Notification;
using System;

namespace DipExecutor.Test
{
    public class TestExecutorBatchStepNotificationFactory : BatchNotifierFactory<StepNotification>
    {
        public override IBatchNotifier<StepNotification> GetBatchNotifier(BatchNotifierType batchNotifierType)
        {
            switch (batchNotifierType)
            {
                case BatchNotifierType.ExecutorLogging:
                    throw new NotImplementedException();

                case BatchNotifierType.ExecutorNotifier:
                    return new BatchNotifierTest();
            }

            throw new NotImplementedException();
        }
    }
}