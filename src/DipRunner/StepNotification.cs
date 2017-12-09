//-----------------------------------------------------------------------
// <copyright file="StepNotification.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using System;

namespace DipRunner
{ 
    public class StepNotification : Step
    {
        public string Machine { get; set; }
        public string Message { get; set; }
        public string MessageGroup { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public NotificationLevel NotificationLevel { get; set; }
        public int NotificationEventId { get; set; }

        public override string ToString()
        {
            var step = $"RunId: {RunId}\nRunName: {RunName}\nStepId: {StepId}\nStepName: {StepName}\nStatus: {Status}\nStepUrl: {StepUrl}\nNotificationUrl: {NotificationUrl}\nLogUrl: {LogUrl}";
            return $"{step}\nMachine: {Machine}\nMessage: {Message}\n MessageGroup: {MessageGroup}\nTimestamp: {Timestamp.ToString("dd/MM/yyyy hh:mm:ss.fff tt")}\nNotificationLevel: {NotificationLevel}\nNotificationEventId: {NotificationEventId}";
        }
    }
}