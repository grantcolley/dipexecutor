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
                case BatchNotifierType.ExecutorLogging:
                    return new ExecutorLogging(null);

                case BatchNotifierType.ExecutorNotifier:
                    return new ExecutorNotifier(httpClientFactory);
            }

            return null;
        }
    }
}