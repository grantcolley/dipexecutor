using DipDistribute;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var step = new Step();
            step.RunId = 101;
            step.RunName = "Test Run 1";
            step.StepId = 1;
            step.StepName = "Step 1";
            step.TargetAssembly = "TestLibrary.dll";
            step.TargetType = "TestLibrary.TestRunner";
            step.LogUrl = "http://localhost:60915/";
            step.Payload = "Hello";
            step.Url = "http://localhost:60915/api/Distributor/Run";
            step.Dependencies = new string[]
            {
                @"C:\GitHub\dipdistribute\TestLibrary\bin\Debug\netcoreapp1.1\DipDistribute.dll",
                @"C:\GitHub\dipdistribute\TestLibrary\bin\Debug\netcoreapp1.1\TestDependency.dll",
                @"C:\GitHub\dipdistribute\TestLibrary\bin\Debug\netcoreapp1.1\TestLibrary.dll"
            };

            Run(step);

            Console.ReadLine();
        }

        private static async void Run(Step step)
        {
            var jsonContent = JsonConvert.SerializeObject(step);
            //var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            //var byteContent = new ByteArrayContent(buffer);

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            await client.PutAsync(step.Url, new StringContent(jsonContent, Encoding.UTF8, "application/json"));
        }

        private static async void DownloadDependency()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri("http://localhost:60915/");
            var stream = client.GetStreamAsync("api/Distributor");

            var file = File.Create(@"C:\Users\grant_000\Desktop\dodgy.pdf");

            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = stream.Result.Read(buffer, 0, buffer.Length)) > 0)
            {
                file.Write(buffer, 0, len);
            }
        }
    }
}