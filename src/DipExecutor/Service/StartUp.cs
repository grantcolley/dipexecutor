//-----------------------------------------------------------------------
// <copyright file="Startup.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using DipExecutor.Notification;
using DipExecutor.Service.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace DipExecutor.Service
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IExecutor, Executor>();
            services.AddTransient<IHttpClientFactory, ExecutorHttpClientFactory>();
            services.AddTransient<IBatchNotifierFactory<StepNotification>, BatchStepNotificationFactory>();

            services.AddTransient<IBatchNotifier<IEnumerable<StepNotification>>, ExecutorPublisher>();

            services.AddSingleton<INotificationPublisher, NotificationPublisher>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory
                .AddConsole(Configuration.GetSection("Logging"))
                .AddDebug();

            app.Map("/run", HandleRun);
            app.Map("/getdependency", HandleFileStrean);
            app.Map("/notify", HandleNotification);
            app.Map("/ping", HandlePing);
        }

        private static void HandleRun(IApplicationBuilder app)
        {
            app.UseRunMiddleware();
        }

        private static void HandleFileStrean(IApplicationBuilder app)
        {
            app.UseFileStreamMiddleware();
        }

        private static void HandleNotification(IApplicationBuilder app)
        {
            app.UseNotificationMiddleware();
        }

        private static void HandlePing(IApplicationBuilder app)
        {
            app.UsePingMiddleware();
        }
    }
}