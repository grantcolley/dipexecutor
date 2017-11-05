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

//        public override IBatchNotifier GetBatchNotifier()
//        {
//            return new ExecutorNotifier(httpClientFactory);
//        }
//    }
//}