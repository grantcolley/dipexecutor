using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace DipDistributor.Middleware
{
    public class FileStreamMiddleware
    {
        public FileStreamMiddleware(RequestDelegate next)
        {
        }

        public async Task Invoke(HttpContext context)
        {
            string file;
            var stream = context.Request.Body;
            stream.Position = 0;
            using (var reader = new StreamReader(stream))
            {
                file = reader.ReadToEnd();
            }
            
            var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            await fileStream.CopyToAsync(context.Response.Body);
        }
    }
}
