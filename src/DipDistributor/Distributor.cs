//-----------------------------------------------------------------------
// <copyright file="Distributor.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using DipRunner;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("DipRunner.Test")]
namespace DipDistributor
{
    /// <summary>
    /// A Distributor runs a <see cref="Step"/> asynchronously.
    /// </summary>
    internal class Distributor : IDistributor
    {
        private string dependencyDirectory;
        private HttpClientFactory httpClientFactory;

        internal Distributor(HttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Runs a step asynchronously.
        /// Running a step involves:
        ///     1. Initialise the step, including downloading dependencies.
        ///     2. Executes the step target type, including dynamically load and 
        ///         initialising an instance of the target type and calling its 
        ///         RunAsync method. 
        ///     3. Running the steps sub steps.
        ///     4. Running the steps transition steps on completion of above.
        /// </summary>
        /// <param name="step">The step to run.</param>
        /// <returns>The step once completed.</returns>
        public async Task<Step> RunAsync(Step step)
        {
            if (step == null)
            {
                throw new Exception(CreateMessage($"Step is null. Machine Name: {Environment.MachineName}"));
            }

            try
            {
                step.Validate();
            }
            catch (Exception ex)
            {
                await LogAsync(step, ex.Message);
                throw;
            }

            return await ProcessStep(step).ConfigureAwait(false);
        }

        private async Task<Step> ProcessStep(Step step)
        {
            var initialised = await InitialiseStepAsync(step).ConfigureAwait(false);

            if (initialised)
            {
                var stepSuccessful = await RunStepAsync(step).ConfigureAwait(false);

                if (stepSuccessful)
                {
                    var subStepsSuccessful = await RunSubStepsAsync(step).ConfigureAwait(false);

                    if (subStepsSuccessful)
                    {
                        var stepCompleted = await CompleteStepAsync(step).ConfigureAwait(false);

                        if (stepCompleted)
                        {
                            var transitionsCompleted = await RunTransitionStepsAsync(step).ConfigureAwait(false); 
                        }
                    }
                }
            }

            return step;
        }

        private async Task<bool> InitialiseStepAsync(Step step)
        {
            try
            {               
                step.Status = StepStatus.Initialise;

                await LogAsync(step);

                dependencyDirectory = Path.Combine(Directory.GetCurrentDirectory(), step.RunName);

                if (!Directory.Exists(dependencyDirectory))
                {
                    await LogAsync(step, $"Create directory {dependencyDirectory}");

                    Directory.CreateDirectory(dependencyDirectory);
                }

                return await DownloadDependenciesAsync(step).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await LogAsync(step, ex.ToString());
                return false;
            }
        }

        private async Task<bool> DownloadDependenciesAsync(Step step)
        {
            try
            {
                if ((step.Dependencies?.Length ?? 0).Equals(0))
                {
                    await LogAsync(step, "No dependencies");
                    return true;
                }

                await LogAsync(step, "Downloading dependencies...");

                var client = httpClientFactory.GetHttpClient();
                client.MaxResponseContentBufferSize = 1000000;

                var dependencies = new List<string>(step.Dependencies);

                IEnumerable<Task<bool>> downloadQuery = from dependency
                                                        in step.Dependencies
                                                        select DownloadDependencyAsync(step, client, step.DependencyUrl, dependency);

                Task<bool>[] downloads = downloadQuery.ToArray();

                var results = await Task.WhenAll(downloads);

                return results.All(r => r == true);
            }
            catch (Exception ex)
            {
                await LogAsync(step, ex.ToString());
                return false;
            }
        }

        private async Task<bool> DownloadDependencyAsync(Step step, HttpClient client, string dependencyUri, string filePath)
        {
            try
            {
                string fullFileName = string.Empty;

                using (var response = await client.PostAsync(dependencyUri, new StringContent(filePath)))
                {
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        var fileName = filePath.Split('\\');
                        fullFileName = Path.Combine(dependencyDirectory, fileName[fileName.Length - 1]);

                        if (File.Exists(fullFileName))
                        {
                            await LogAsync(step, $"File already exists: {fullFileName}");
                            return true;
                        }

                        using (var file = File.Create(fullFileName))
                        {
                            byte[] buffer = new byte[8 * 1024];
                            int len;
                            while ((len = stream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                file.Write(buffer, 0, len);
                            }
                        }
                    }
                }

                await LogAsync(step, $"Downloaded: {fullFileName}");
                return true;
            }
            catch(Exception ex)
            {
                await LogAsync(step, ex.ToString());
                return false;
            }
        }

        private async Task<bool> RunStepAsync(Step step)
        {
            try
            {
                step.Status = StepStatus.InProgress;

                await LogAsync(step);

                if (string.IsNullOrWhiteSpace(step.TargetAssembly))
                {
                    await LogAsync(step, "TargetAssembly is missing.");
                    return true;
                }

                if (string.IsNullOrWhiteSpace(step.TargetType))
                {
                    await LogAsync(step, "TargetType is missing.");
                    return true;
                }

                var dependencies = GetDependencyAssemblyNames(step);

                var assemblyLoader = new AssemblyLoader(dependencyDirectory, dependencies);
                var assembly = assemblyLoader.LoadFromAssemblyPath(Path.Combine(dependencyDirectory, step.TargetAssembly));

                var type = assembly.GetType(step.TargetType);
                dynamic obj = Activator.CreateInstance(type);

                await LogAsync(step, $"Execute {step.TargetType}.RunAsync() --> {step?.Payload}");

                var result = await obj.RunAsync(step);

                await LogAsync(step, $"Executed {step.TargetType}.RunAsync() --> {step?.Payload}");

                return true;
            }
            catch (Exception ex)
            {
                await LogAsync(step, ex.ToString());
                return false;
            }
        }

        internal async Task<bool> RunSubStepsAsync(Step step)
        {
            return await RunStepsAsync(step, step.SubSteps, "sub");
        }

        internal async Task<bool> RunTransitionStepsAsync(Step step)
        {
            return await RunStepsAsync(step, step.TransitionSteps, "transition");
        }

        internal async Task<bool> CompleteStepAsync(Step step)
        {
            try
            {
                step.Status = StepStatus.Complete;

                await LogAsync(step);

                return true;
            }
            catch (Exception ex)
            {
                await LogAsync(step, ex.ToString());
                return false;
            }
        }

        internal IEnumerable<Step> SetUrl(IEnumerable<Step> steps, IEnumerable<string> urls)
        {
            if (urls == null
                || urls.Count() == 0)
            {
                return steps;
            }

            var maxUrl = urls.Count() - 1;
            var urlIndex = 0;

            foreach (var step in steps)
            {
                if (string.IsNullOrEmpty(step.StepUrl))
                {
                    step.StepUrl = urls.ElementAt<string>(urlIndex);

                    if (urlIndex == maxUrl)
                    {
                        urlIndex = 0;
                    }
                    else
                    {
                        urlIndex++;
                    }
                }

                if (step.Urls == null
                    || step.Urls.Equals(0))
                {
                    step.Urls = urls.ToArray<string>();
                }
            }

            return steps;
        }

        internal async Task<Step> DistributeStepAsync(Step step)
        {
            var jsonContent = JsonConvert.SerializeObject(step);
            var client = httpClientFactory.GetHttpClient();
            using (var response = await client.PostAsync(step.StepUrl, new StringContent(jsonContent, Encoding.UTF8, "application/json")))
            {
                var content = await response.Content.ReadAsStringAsync();
                var responseStep = JsonConvert.DeserializeObject<Step>(content);
                return responseStep;
            }
        }

        internal IList<string> GetDependencyAssemblyNames(Step step)
        {
            var dependencies = new List<string>();
            foreach (string filePath in step.Dependencies)
            {
                var filePathSplit = filePath.Split('\\');
                var fileName = filePathSplit[filePathSplit.Length - 1];
                var name = fileName.Substring(0, fileName.LastIndexOf('.'));
                dependencies.Add(name);
            }

            return dependencies;
        }

        internal async Task LogAsync(Step step, string message = "")
        {
            var logMessage = CreateMessage(step, message);
            var client = httpClientFactory.GetHttpClient();
            var response = await client.PostAsync(step.LogUrl, new StringContent(JsonConvert.SerializeObject(logMessage), Encoding.UTF8, "application/json"));
            response.Dispose();
        }

        internal string CreateMessage(string message)
        {
            return CreateMessage(new Step(), message);
        }

        internal string CreateMessage(Step step, string message)
        {
            var logMessage = $"{DateTime.Now}   {Environment.MachineName}   RunId: {step.RunId}; Run Name: {step.RunName}; StepId: {step.StepId}; Step Name: {step.StepName}; Step Status: {step.Status}";

            if (!string.IsNullOrWhiteSpace(message))
            {
                logMessage += $"; Message: {message}";
            }

            return logMessage;
        }

        private async Task<bool> RunStepsAsync(Step step, IEnumerable<Step> steps, string type)
        {
            try
            {
                if (steps == null
                    || !steps.Any())
                {
                    await LogAsync(step, $"RunStepsAsync - No {type} steps");
                    return true;
                }

                await LogAsync(step, $"RunStepsAsync - {type} steps");

                var stepsToRun = SetUrl(steps, step.Urls);

                IEnumerable<Task<Step>> runningStepsQuery = from s in stepsToRun select DistributeStepAsync(s);

                Task<Step>[] runningSteps = runningStepsQuery.ToArray();

                var results = await Task.WhenAll(runningSteps);

                if (results.All(r => r.Status == StepStatus.Complete))
                {
                    await LogAsync(step, $"RunStepsAsync - {type} steps completed");
                    return true;
                }
                else
                {
                    await LogAsync(step, $"RunStepsAsync - Not all {type} steps completed");
                    return false;
                }
            }
            catch (Exception ex)
            {
                await LogAsync(step, $"RunStepsAsync - {ex.ToString()}");
                return false;
            }
        }
    }
}
