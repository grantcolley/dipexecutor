using DipRunner;
using Microsoft.Extensions.Logging;
using System;

namespace DipExecutor.Service.Logging
{
    public struct LogMessage
    {
        public int RunId { get; set; }
        public string RunName { get; set; }
        public int StepId { get; set; }
        public string StepName {get;set;}
        public StepStatus Status { get; set; }
        public string StepUrl { get; set; }
        public string LogUrl { get; set; }
        public string Machine { get; set; }
        public string Message { get; set; }
        public string MessageGroup { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public LogLevel LogLevel { get; set; }
        public int EventId { get; set; }
    }
}