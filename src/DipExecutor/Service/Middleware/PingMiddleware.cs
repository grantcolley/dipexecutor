//-----------------------------------------------------------------------
// <copyright file="PingMiddleware.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace DipExecutor.Service.Middleware
{
    public class PingMiddleware
    {
        public PingMiddleware(RequestDelegate next)
        {
        }

        public async Task Invoke(HttpContext context)
        {
            await context.Response.WriteAsync($"{Environment.MachineName} DipExecutor Is Alive!");
        }
    }
}
