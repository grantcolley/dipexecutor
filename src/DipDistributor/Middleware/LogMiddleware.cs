using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace DipDistributor.Middleware
{
    public class LogMiddleware
    {
        public LogMiddleware(RequestDelegate next)
        {
        }

        public async Task Invoke(HttpContext context)
        {
            string body;
            var stream = context.Request.Body;
            using (var reader = new StreamReader(stream))
            {
                body = await reader.ReadToEndAsync();
            }

            Logger.Log(body);
        }
    }
}
