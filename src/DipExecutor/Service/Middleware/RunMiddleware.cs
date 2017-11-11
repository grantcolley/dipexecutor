//-----------------------------------------------------------------------
// <copyright file="RunMiddleware.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using DipRunner;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DipExecutor.Service.Middleware
{
    public class RunMiddleware
    {
        public RunMiddleware(RequestDelegate next)
        {
        }

        public async Task Invoke(HttpContext context, IExecutor executor)
        {
            string body;
            var stream = context.Request.Body;
            using (var reader = new StreamReader(stream))
            {
                body = await reader.ReadToEndAsync();
            }

            var step = JsonConvert.DeserializeObject<Step>(body);
            var response = await executor.RunAsync(step);
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response), Encoding.UTF8);
        }
    }
}
