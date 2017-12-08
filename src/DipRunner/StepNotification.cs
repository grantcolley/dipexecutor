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
    }
}