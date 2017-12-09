using System;
using View4Logs.Common.Interfaces;

namespace View4Logs.Common.Data
{
    public class LogEvent
    {
        public ILogSource Source { get; set; }

        public string Message { get; set; }

        public LogLevel Level { get; set; }

        public string LoggerName { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
