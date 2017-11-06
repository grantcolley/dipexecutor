//using DipExecutor.Service;

//namespace DipExecutor.Notification
//{
//    public class ExecutorBatchNotifierFactory : BatchNotifierFactory
//    {
//        private readonly IHttpClientFactory httpClientFactory;

//        public ExecutorBatchNotifierFactory(IHttpClientFactory httpClientFactory)
//        {
//            this.httpClientFactory = httpClientFactory;
//        }

//        public override IBatchNotifier<T> GetBatchNotifier<T>(BatchNotifierType batchNotifierType)
//        {
//            return new ExecutorNotifier(httpClientFactory);
//        }
//    }
//}