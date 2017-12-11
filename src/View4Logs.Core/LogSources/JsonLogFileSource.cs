using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using View4Logs.Common.Data;

namespace View4Logs.Core.LogSources
{
    public class JsonLogFileSource : JsonLogFileSourceBase
    {
        private static readonly Dictionary<string, LogLevel> LogLevelMapping = new Dictionary<string, LogLevel>
        {
            { "TRACE", LogLevel.Trace },
            { "DEBUG", LogLevel.Debug },
            { "INFO", LogLevel.Info },
            { "WARN", LogLevel.Warn },
            { "ERROR", LogLevel.Error },
            { "FATAL", LogLevel.Fatal },
        };

        public JsonLogFileSource(string path)
            : base(path)
        {
        }

        protected override LogEvent ConvertObjectToLogEvent(JObject obj)
        {
            var logEvent = new LogEvent
            {
                Level = LogLevelMapping[obj["level"].ToString()],
                Message = obj["message"].ToString(),
                TimeStamp = DateTime.Parse(obj["time"].ToString()),
                Source = this
            };

            return logEvent;
        }
    }
}
