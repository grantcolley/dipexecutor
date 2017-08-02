using DipDistribute;
using Newtonsoft.Json;
using System;
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
            step.LogUri = "http://localhost:60915/";
            step.Payload = "Hello";
            step.Uri = "http://localhost:60915/api/Distributor/Run";
            step.DependencyUri = "http://localhost:60915/api/Distributor/GetDependency";
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
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            await client.PutAsync(step.Uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));
        }
    }
}