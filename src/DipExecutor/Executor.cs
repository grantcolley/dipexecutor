//-----------------------------------------------------------------------
// <copyright file="Executor.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using DipExecutor.Service;
using DipExecutor.Notification;
using DipExecutor.Utilities;
using DipRunner;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DipExecutor.Test")]
namespace DipExecutor
{
    /// <summary>
    /// A Executor runs a <see cref="Step"/> asynchronously.
    /// </summary>
    public class Executor : IExecutor
    {
        private readonly IBatchNotifier<StepNotification> batchNotifier;
        private readonly IHttpClientFactory httpClientFactory;

        public Executor(IHttpClientFactory httpClientFactory, IBatchNotifierFactory<StepNotification> batchNotifierFactory)
        {
            this.httpClientFactory = httpClientFactory;
            this.batchNotifier = batchNotifierFactory.GetBatchNotifier(BatchNotifierType.ExecutorNotifier);
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
                return null;
            }

            try
            {
                step.Validate();

                return await ProcessStepAsync(step).ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                Notify(NotificationLevel.Error, NotificationEvent.RunStepAsync, step, ex.ToString());
                return null;
            }
        }

        internal async Task<Step> ProcessStepAsync(Step step)
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

        internal async Task<bool> InitialiseStepAsync(Step step)
        {
            try
            {               
                step.Status = StepStatus.Initialise;

                Notify(NotificationLevel.Information, NotificationEvent.InitialiseStepAsync, step);
                
                return await DownloadDependenciesAsync(step).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Notify(NotificationLevel.Error, NotificationEvent.InitialiseStepAsync, step, ex.ToString());
                return false;
            }
        }

        internal void CreateAssemblyPath(Step step)
        {
            if (string.IsNullOrWhiteSpace(step.AssemblyPath))
            {
                step.AssemblyPath = Path.Combine(Directory.GetCurrentDirectory(), "downloads", Guid.NewGuid().ToString());
            }

            if (!Directory.Exists(step.AssemblyPath))
            {
                Directory.CreateDirectory(step.AssemblyPath);
            }
        }
        
        internal async Task<bool> DownloadDependenciesAsync(Step step)
        {
            try
            {
                if ((step.Dependencies?.Length ?? 0).Equals(0))
                {
                    return true;
                }

                CreateAssemblyPath(step);

                Notify(NotificationLevel.Information, NotificationEvent.DownloadDependenciesAsync, step, "Downloading dependencies...");

                var client = httpClientFactory.GetHttpClient(HttpClientResponseContentType.StreamContent);                
                var dependencies = new List<string>(step.Dependencies);

                IEnumerable<Task<bool>> downloadQuery = from dependency
                                                        in step.Dependencies
                                                        select DownloadDependencyAsync(step, client, dependency);

                Task<bool>[] downloads = downloadQuery.ToArray();

                var results = await Task.WhenAll(downloads);

                return results.All(r => r == true);
            }
            catch (Exception ex)
            {
                Notify(NotificationLevel.Error, NotificationEvent.DownloadDependenciesAsync, step, ex.ToString());
                return false;
            }
        }

        internal async Task<bool> DownloadDependencyAsync(Step step, HttpClient client, string filePath)
        {
            try
            {
                string fileName = string.Empty;

                using (var response = await client.PostAsync(step.DependencyUrl, new StringContent(filePath)))
                {
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        var filePathSplit = filePath.Split('\\');
                        fileName = filePathSplit[filePathSplit.Length-1];

                        var fullFileName = Path.Combine(step.AssemblyPath, fileName);

                        if (File.Exists(fullFileName))
                        {
                            Notify(NotificationLevel.Information, NotificationEvent.DownloadDependencyAsync, step, $"File already exists: {fileName}");
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

                Notify(NotificationLevel.Information, NotificationEvent.DownloadDependencyAsync, step, $"Downloaded: {fileName}");
                return true;
            }
            catch(Exception ex)
            {
                Notify(NotificationLevel.Error, NotificationEvent.DownloadDependencyAsync, step, ex.ToString());
                return false;
            }
        }

        internal async Task<bool> RunStepAsync(Step step)
        {
            try
            {
                step.Status = StepStatus.InProgress;

                Notify(NotificationLevel.Information, NotificationEvent.RunStepAsync, step);

                if (string.IsNullOrWhiteSpace(step.TargetAssembly))
                {
                    Notify(NotificationLevel.Information, NotificationEvent.RunStepAsync, step, "No TargetAssembly");
                    return true;
                }

                if (string.IsNullOrWhiteSpace(step.TargetType))
                {
                    Notify(NotificationLevel.Information, NotificationEvent.RunStepAsync, step, "No TargetType");
                    return true;
                }
                else
                {
                    Notify(NotificationLevel.Information, NotificationEvent.RunStepAsync, step, $"Executing {step.TargetType}");
                }

                var dependencies = GetDependencyAssemblyNames(step);

                var assemblyLoader = new AssemblyLoader(step.AssemblyPath, dependencies);
                var assembly = assemblyLoader.LoadFromMemoryStream(Path.Combine(step.AssemblyPath, step.TargetAssembly));
                var type = assembly.GetType(step.TargetType);
                dynamic obj = Activator.CreateInstance(type);

                var result = await obj.RunAsync(step);

                return true;
            }
            catch (Exception ex)
            {
                Notify(NotificationLevel.Error, NotificationEvent.RunStepAsync, step, ex.ToString());
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

                if (!step.SkipAssemblyCleanup)
                {
                    await Cleanup(step);
                }

                Notify(NotificationLevel.Information, NotificationEvent.CompleteStepAsync, step);

                return true;
            }
            catch (Exception ex)
            {
                Notify(NotificationLevel.Error, NotificationEvent.CompleteStepAsync, step, ex.ToString());
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

        internal void Notify(NotificationLevel notificationLevel, int notificationEventId, Step step, string message = "")
        {
            var notification = step.CreateStepNotification(notificationLevel, notificationEventId, message);
            batchNotifier.AddNotification(notification);
        }

        internal async Task Cleanup(Step step)
        {
            if (string.IsNullOrWhiteSpace(step.AssemblyPath))
            {
                return;
            }

            try
            {
                if (Directory.Exists(step.AssemblyPath))
                {
                    foreach (var file in Directory.GetFiles(step.AssemblyPath))
                    {
                        File.SetAttributes(file, FileAttributes.Normal);
                        File.Delete(file);
                    }

                    Directory.Delete(step.AssemblyPath);
                }
            }
            catch(Exception ex)
            {
                Notify(NotificationLevel.Error, NotificationEvent.Cleanup, step, ex.ToString());
            }
        }

        private async Task<bool> RunStepsAsync(Step step, IEnumerable<Step> steps, string type)
        {
            try
            {
                if (steps == null
                    || !steps.Any())
                {
                    return true;
                }

                Notify(NotificationLevel.Information, NotificationEvent.RunStepsAsync, step, $" Running {steps.Count()} {type} step(s)");

                var stepsToRun = SetUrl(steps, step.Urls);

                IEnumerable<Task<Step>> runningStepsQuery = from s in stepsToRun select DistributeStepAsync(s);

                Task<Step>[] runningSteps = runningStepsQuery.ToArray();

                var results = await Task.WhenAll(runningSteps);
                
                if (results.All(r => r.Status == StepStatus.Complete))
                {
                    Notify(NotificationLevel.Information, NotificationEvent.RunStepsAsync, step, $" {steps.Count()} {type} steps completed");
                    return true;
                }
                else
                {
                    Notify(NotificationLevel.Error, NotificationEvent.RunStepsAsync, step, $"{results.Count(r => r.Status != StepStatus.Complete)} {type} steps failed to complete");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Notify(NotificationLevel.Error, NotificationEvent.RunStepsAsync, step, $"RunStepsAsync - {ex.ToString()}");
                return false;
            }
        }
    }
}
