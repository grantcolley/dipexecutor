namespace DipExecutor.Service.Logging
{
    public class LogEvent
    {
        public const int InitialiseStepAsync = 1;
        public const int DownloadDependenciesAsync = 2;
        public const int DownloadDependencyAsync = 3;
        public const int RunStepAsync = 4;
        public const int RunStepsAsync = 5;
        public const int CompleteStepAsync = 6;
        public const int Cleanup = 7;
    }
}