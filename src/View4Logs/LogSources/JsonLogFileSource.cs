using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using View4Logs.Common.Data;

namespace View4Logs.LogSources
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

        protected override LogMessage ConvertObjectToLogMessage(JObject obj)
        {
            var logMessage = new LogMessage
            {
                Level = LogLevelMapping[obj["level"].ToString()],
                Message = obj["message"].ToString(),
                TimeStamp = DateTime.Parse(obj["time"].ToString())
            };

            return logMessage;
        }
    }
}
