//-----------------------------------------------------------------------
// <copyright file="FileStreamMiddleware.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace DipExecutor.Service.Middleware
{
    public class FileStreamMiddleware
    {
        public FileStreamMiddleware(RequestDelegate next)
        {
        }

        public async Task Invoke(HttpContext context)
        {
            string body;
            var stream = context.Request.Body;
            using (var reader = new StreamReader(stream))
            {
                body = await reader.ReadToEndAsync();
            }

            using (var fileStream = new FileStream(body, FileMode.Open, FileAccess.Read))
            {
                await fileStream.CopyToAsync(context.Response.Body);
            }
        }
    }
}
