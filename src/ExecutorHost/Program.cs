using DipExecutor.Service;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ExecutorHost
{
    class Program
    {
        static void Main(string[] args)
        {
            string url;

            if (args == null
                || args.Length.Equals(0))
            {
                url = "http://+:5000";
            }
            else
            {
                url = args[0];
            }

            var webHost = WebHost.CreateDefaultBuilder()
                .UseUrls(url)
                .UseExecutorStartup()
                .Build();

            var task = webHost.RunAsync();
            task.GetAwaiter().GetResult();
        }
    }
}