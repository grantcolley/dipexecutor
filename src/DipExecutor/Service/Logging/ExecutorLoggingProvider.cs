using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DipExecutor.Service.Logging
{
    public class ExecutorLoggingProvider : BatchingLoggerProvider
    {
        private ExecutorLoggerOptions options;
        private IHttpClientFactory httpClientFactory;

        public ExecutorLoggingProvider(IOptions<ExecutorLoggerOptions> options, IHttpClientFactory httpClientFactory) : base(options)
        {
            this.options = options.Value;
            this.httpClientFactory = httpClientFactory;
        }

        protected override async Task WriteMessagesAsync(IEnumerable<LogMessage> messages, CancellationToken cancellationToken)
        {
            var client = httpClientFactory.GetHttpClient();

            foreach (var group in messages.GroupBy(m => m.MessageGroup))
            {
                var logMessages = group.ToList();
                var jsonContent = JsonConvert.SerializeObject(logMessages);
                using (var response = await client.PostAsync(logMessages.First<LogMessage>().LogUrl, new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json")))
                {
                    //var content = await response.Content.ReadAsStringAsync();
                    //var responseStep = JsonConvert.DeserializeObject<Step>(content);
                }
            }
        }
    }
}