using System;

namespace DipExecutor.Service.Logging
{
    public struct LogMessage
    {
        public string MessageGroup { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Message { get; set; }
    }
}
