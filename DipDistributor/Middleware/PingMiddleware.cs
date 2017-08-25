using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace DipDistributor.Middleware
{
    public class PingMiddleware
    {
        public PingMiddleware(RequestDelegate next)
        {
        }

        public async Task Invoke(HttpContext context)
        {
            await context.Response.WriteAsync($"{Environment.MachineName} Alive");
        }
    }
}
