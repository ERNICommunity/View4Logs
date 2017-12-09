using System;
using System.Collections.Generic;
using View4Logs.Common.Interfaces;

namespace View4Logs.Common.Data
{
    public class LogEvent
    {
        public ILogSource Source { get; set; }

        public string Message { get; set; }

        public LogLevel Level { get; set; }

        public string Logger { get; set; }

        public DateTime TimeStamp { get; set; }

        public List<ActivityInfo> Activities { get; set; }

        public string Exception { get; set; }

        public CodeInfo Code { get; set; }

        public ProcessInfo Process { get; set; }
    }
}
