using Microsoft.AspNetCore.Builder;

namespace DipDistributor.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseRunMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RunMiddleware>();
        }

        public static IApplicationBuilder UseFileStreamMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FileStreamMiddleware>();
        }

        public static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder builder            )
        {
            return builder.UseMiddleware<LogMiddleware>();
        }

        public static IApplicationBuilder UsePingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PingMiddleware>();
        }
    }
}