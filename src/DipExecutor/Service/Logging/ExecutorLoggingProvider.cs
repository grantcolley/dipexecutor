using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DipExecutor.Service.Logging
{
    public class ExecutorLoggingProvider : BatchingLoggerProvider
    {
        public string uri;

        public ExecutorLoggingProvider(IOptions<ExecutorLoggerOptions> options) : base(options)
        {
            var loggerOptions = options.Value;
            uri = loggerOptions.Uri;
        }

        protected override async Task WriteMessagesAsync(IEnumerable<LogMessage> messages, CancellationToken cancellationToken)
        {
            foreach (var group in messages.GroupBy(m => m.MessageGroup))
            {
                // Do stuff here...
            }
        }
    }
}