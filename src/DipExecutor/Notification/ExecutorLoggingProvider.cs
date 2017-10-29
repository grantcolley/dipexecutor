//using Microsoft.Extensions.Options;
//using Newtonsoft.Json;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Threading;
//using System.Threading.Tasks;

//namespace DipExecutor.Service.Logging
//{
//    public class ExecutorLoggingProvider : BatchingLoggerProvider
//    {
//        private ExecutorLoggerOptions options;

//        public ExecutorLoggingProvider(IOptions<ExecutorLoggerOptions> options) : base(options)
//        {
//            this.options = options.Value;
//        }

//        protected override async Task WriteMessagesAsync(IEnumerable<LogMessage> messages, CancellationToken cancellationToken)
//        {
//            var client = options.HttpClientFactory.GetHttpClient();

//            foreach (var group in messages.GroupBy(m => m.MessageGroup))
//            {
//                var logMessages = group.ToList();
//                var jsonContent = JsonConvert.SerializeObject(logMessages);
//                using (var response = await client.PostAsync(logMessages.First<LogMessage>().LogUrl, new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json")))
//                {
//                    //var content = await response.Content.ReadAsStringAsync();
//                    //var responseStep = JsonConvert.DeserializeObject<Step>(content);
//                }
//            }
//        }
//    }
//}