//-----------------------------------------------------------------------
// <copyright file="StepNotification.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using DipRunner;
using System;

namespace DipExecutor.Notification
{
    public struct StepNotification
    {
        public int RunId { get; set; }
        public string RunName { get; set; }
        public int StepId { get; set; }
        public string StepName {get;set;}
        public StepStatus Status { get; set; }
        public string StepUrl { get; set; }
        public string NotificationUrl { get; set; }
        public string LoggingUrl { get; set; }
        public string Machine { get; set; }
        public string Message { get; set; }
        public string MessageGroup { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public NotificationLevel NotificationLevel { get; set; }
        public int NotificationEventId { get; set; }
    }
}