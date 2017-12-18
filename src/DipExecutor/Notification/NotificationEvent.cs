//-----------------------------------------------------------------------
// <copyright file="NotificationEvent.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

namespace DipExecutor.Notification
{
    public class NotificationEvent
    {
        public const int RunAsync = 0;
        public const int InitialiseStepAsync = 1;
        public const int DownloadDependenciesAsync = 2;
        public const int DownloadDependencyAsync = 3;
        public const int RunStepAsync = 4;
        public const int RunStepsAsync = 5;
        public const int CompleteStepAsync = 6;
        public const int Cleanup = 7;
    }
}
