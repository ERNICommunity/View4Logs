using System;
using System.Linq;
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
                Logger = obj["logger"].ToString(),
                Message = obj["message"].ToString(),
                Exception = obj["exception"]?.ToString(),
                TimeStamp = DateTime.Parse(obj["time"].ToString()),
                Source = this
            };

            if (obj.TryGetValue("activities", out var activities))
            {
                var activityList = activities
                    .Children<JObject>()
                    .Select(ac => new ActivityInfo { Id = ac["Id"].ToString(), Name = ac["Name"].ToString() })
                    .ToList();

                logEvent.Activities = activityList;
            }

            return logEvent;
        }
    }
}
