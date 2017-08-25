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

        private object locker = new object();

        public async Task Invoke(HttpContext context)
        {
            string body;
            var stream = context.Request.Body;
            using (var reader = new StreamReader(stream))
            {
                body = await reader.ReadToEndAsync();
            }

            // TODO: get from config...
            string path = @"C:\GitHub\dipdistributor\DipDistributor.txt";

            // TODO: clean this stuff up. Testing only.
            lock (locker)
            {
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(body);
                    }

                    return;
                }

                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(body);
                }
            }
        }
    }
}
