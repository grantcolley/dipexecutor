//-----------------------------------------------------------------------
// <copyright file="DistributorService.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace DipDistributor
{
    internal class DistributorService
    {
        internal void Run(string url)
        {
            var webHost = WebHost.CreateDefaultBuilder()
                .UseUrls(url)
                .UseStartup<Startup>()
                .Build();

            var task = webHost.RunAsync();
            task.GetAwaiter().GetResult();
        }
    }
}
