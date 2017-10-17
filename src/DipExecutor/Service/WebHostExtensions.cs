//-----------------------------------------------------------------------
// <copyright file="IExecutor.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using DipExecutor.Service.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace DipExecutor.Service
{
    /// <summary>
    /// DipExecutor extensions for <see cref="WebHost"/>
    /// </summary>
    public static class WebHostExtensions
    {
        public static IWebHostBuilder UseExecutorStartup(this IWebHostBuilder webHost)
        {
            return webHost.UseStartup<Startup>();
        }

        public static ILoggingBuilder AddExecutor(this ILoggingBuilder builder)
        {
            builder.Services.AddSingleton<ILoggerProvider, ExecutorLoggingProvider>();
            return builder;
        }

        public static ILoggingBuilder AddExecutor(this ILoggingBuilder builder, Action<ExecutorLoggerOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            builder.AddExecutor();
            builder.Services.Configure(options);
            return builder;
        }
    }
}