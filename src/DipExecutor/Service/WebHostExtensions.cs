//-----------------------------------------------------------------------
// <copyright file="WebHostExtensions.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.Hosting;

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
    }
}