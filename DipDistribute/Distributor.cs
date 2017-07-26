namespace DipDistribute
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class Distributor : IDistributor
    {
        public async Task<Step> RunAsync(Step step)
        {
            return await Task.Run<Step>(() => ProcessStep(step)).ConfigureAwait(false);
        }

        private Step ProcessStep(Step step)
        {
            if (InitialiseStep(step))
            {
                if (RunStep(step))
                {
                    CompleteStep(step);
                }
            }

            return step;
        }

        private bool InitialiseStep(Step step)
        {
            try
            {
                if (step == null)
                {
                    Log(new Step(), $"Step is null. Machine Name: {Environment.MachineName}");
                    return false;
                }

                // TODO: Check for other minimum requirements for a step to continue e.g. Name etc...
                
                step.Status = StepStatus.Initialise;

                Log(step);

                if (step.Dependencies == null
                    || step.Dependencies.Length == 0)
                {
                    Log(step, "No dependencies");
                    return true;
                }

                return DownloadDependencies(step);
            }
            catch (Exception ex)
            {
                Log(step, ex.ToString());
                return false;
            }
        }

        private bool DownloadDependencies(Step step)
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

                foreach (var file in step.Dependencies)
                {
                    // TODO: Download each dependency...
                }

                return true;
            }
            catch (Exception ex)
            {
                Log(step, ex.ToString());
                return false;
            }
        }

        private bool RunStep(Step step)
        {
            try
            {
                step.Status = StepStatus.InProgress;

                Log(step);

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

        private void CompleteStep(Step step)
        {
            try
            {
                step.Status = StepStatus.Complete;

                Log(step);

                // TODO: Run transitions...
            }
            catch (Exception ex)
            {
                Log(step, ex.ToString());
            }
        }

        private void Log(Step step, string message = "")
        {
            var log = $"RunId: {step.RunId}; Run Name: {step.RunName}; StepId: {step.StepId}; Step Name: {step.StepName}; Step Status: {step.Status}";

            if (!string.IsNullOrWhiteSpace(message))
            {
                log += $"; Message: {message}";
            }

            // TODO: log here...
        }
    }
}
