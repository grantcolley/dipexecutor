using DipExecutor.Service.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace DipExecutor.Service.Middleware
{
    public class LogMiddleware
    {
        ILogger logger;

        public LogMiddleware(RequestDelegate next, ILoggerFactory logger)
        {
            this.logger = logger.CreateLogger<BatchingLogger>();
        }

        public async Task Invoke(HttpContext context)
        {
            string body;
            var stream = context.Request.Body;
            using (var reader = new StreamReader(stream))
            {
                body = await reader.ReadToEndAsync();
            }

        }
    }
}
