using Microsoft.AspNetCore.Builder;

namespace DipDistributor.Middleware
{
    internal static class MiddlewareExtensions
    {
        internal static IApplicationBuilder UseRunMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RunMiddleware>();
        }

        internal static IApplicationBuilder UseFileStreamMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FileStreamMiddleware>();
        }

        internal static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder builder            )
        {
            return builder.UseMiddleware<LogMiddleware>();
        }

        internal static IApplicationBuilder UsePingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PingMiddleware>();
        }
    }
}