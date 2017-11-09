using Microsoft.AspNetCore.Builder;

namespace DipExecutor.Service.Middleware
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

        internal static IApplicationBuilder UseNotificationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<NotificationMiddleware>();
        }

        internal static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }

        internal static IApplicationBuilder UsePingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PingMiddleware>();
        }
    }
}