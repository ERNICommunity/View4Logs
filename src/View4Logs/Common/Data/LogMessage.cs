using System;

namespace View4Logs.Common.Data
{
    public class LogMessage
    {
        public string Message { get; set; }

        public LogLevel Level { get; set; }

        public string LoggerName { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
