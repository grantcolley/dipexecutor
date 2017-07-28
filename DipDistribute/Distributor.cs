namespace DipDistribute
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    public class Distributor : IDistributor
    {
        private HttpClient logClient;
            
        public async Task<Step> RunAsync(Step step)
        {
            if (step == null)
            {
                throw new Exception(CreateMessage($"Step is null. Machine Name: {Environment.MachineName}"));
            }

            if (string.IsNullOrWhiteSpace(step.RunName))
            {
                throw new Exception(CreateMessage(step, "Run Name is missing."));
            }

            if (string.IsNullOrWhiteSpace(step.StepName))
            {
                throw new Exception(CreateMessage(step, "Step Name is missing."));
            }

            if (string.IsNullOrWhiteSpace(step.LogUrl))
            {
                throw new Exception(CreateMessage(step, "Log Url is missing."));
            }

            logClient = new HttpClient();
            logClient.DefaultRequestHeaders.Accept.Clear();
            logClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            logClient.BaseAddress = new Uri(step.LogUrl);

            return await ProcessStep(step).ConfigureAwait(false);
        }

        private async Task<Step> ProcessStep(Step step)
        {
            var initialised = await InitialiseStepAsync(step).ConfigureAwait(false);

            if (initialised)
            {
                var success = await RunStepAsync(step).ConfigureAwait(false);

                if (success)
                {
                    var completed = await CompleteStepAsync(step).ConfigureAwait(false);
                }
            }

            return step;
        }

        private async Task<bool> InitialiseStepAsync(Step step)
        {
            try
            {               
                step.Status = StepStatus.Initialise;

                Log(step);

                if (step.Dependencies == null
                    || step.Dependencies.Length == 0)
                {
                    Log(step, "No dependencies");
                    return true;
                }

                return await DownloadDependenciesAsync(step).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log(step, ex.ToString());
                return false;
            }
        }

        private async Task<bool> DownloadDependenciesAsync(Step step)
        {
            try
            {
                Log(step, "Downloading dependencies...");

                var targetDirectory = Path.Combine(Directory.GetCurrentDirectory(), step.RunName);
                if (!Directory.Exists(targetDirectory))
                {
                    Log(step, $"Create directory {targetDirectory}");

                    Directory.CreateDirectory(targetDirectory);
                }

                // TODO: make async...

                foreach (var filePath in step.Dependencies)
                {
                    var file = new FileInfo(filePath);
                    File.Copy(file.FullName, Path.Combine(targetDirectory, file.Name));
                }

                return true;
            }
            catch (Exception ex)
            {
                Log(step, ex.ToString());
                return false;
            }
        }

        private async Task<bool> RunStepAsync(Step step)
        {
            try
            {
                step.Status = StepStatus.InProgress;

                Log(step);

                // TODO: Make async...

                // TODO: Load assembly here...

                // TODO: Execute Run here...

                return true;
            }
            catch (Exception ex)
            {
                Log(step, ex.ToString());
                return false;
            }
        }

        private async Task<bool> CompleteStepAsync(Step step)
        {
            try
            {
                step.Status = StepStatus.Complete;

                Log(step);

                // TODO: Run transitions asynchronously...

                return true;
            }
            catch (Exception ex)
            {
                Log(step, ex.ToString());
                return false;
            }
        }

        private void Log(Step step, string message = "")
        {
            Task.Run(() =>
            {
                var logMessage = CreateMessage(step, message);
                logClient.PostAsync("api/log", new StringContent(logMessage));
            });
        }

        private string CreateMessage(string message)
        {
            return CreateMessage(new Step(), message);
        }

        private string CreateMessage(Step step, string message)
        {
            var logMessage = $"RunId: {step.RunId}; Run Name: {step.RunName}; StepId: {step.StepId}; Step Name: {step.StepName}; Step Status: {step.Status}";

            if (!string.IsNullOrWhiteSpace(message))
            {
                logMessage += $"; Message: {message}";
            }

            return logMessage;
        }
    }
}
