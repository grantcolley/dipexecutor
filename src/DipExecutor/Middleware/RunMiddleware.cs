using DipRunner;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DipExecutor.Middleware
{
    public class RunMiddleware
    {
        public RunMiddleware(RequestDelegate next)
        {
        }

        public async Task Invoke(HttpContext context, IExecutor distributor)
        {
            string body;
            var stream = context.Request.Body;
            using (var reader = new StreamReader(stream))
            {
                body = await reader.ReadToEndAsync();
            }

            var step = JsonConvert.DeserializeObject<Step>(body);
            var response = await distributor.RunAsync(step);
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response), Encoding.UTF8);
        }
    }
}
