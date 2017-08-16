//-----------------------------------------------------------------------
// <copyright file="DistributorService.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace DipDistributor
{
    public class DistributorService
    {
        public void Run(string Url)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls(Url)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}
