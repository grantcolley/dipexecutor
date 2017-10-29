﻿//-----------------------------------------------------------------------
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
            services.AddSingleton<IBatchNotifier, BatchNotifier>();
            services.AddTransient<IHttpClientFactory, ExecutorHttpClientFactory>();
            services.AddTransient<IExecutor, Executor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory
                .AddConsole(Configuration.GetSection("Logging"))
                .AddDebug();

            app.Map("/run", HandleRun);
            app.Map("/getdependency", HandleFileStrean);
            app.Map("/log", HandleLog);
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

        private static void HandleLog(IApplicationBuilder app)
        {
            app.UseLogMiddleware();
        }

        private static void HandlePing(IApplicationBuilder app)
        {
            app.UsePingMiddleware();
        }
    }
}