using System.Collections.Generic;
using System.Xml.Linq;
using View4Logs.Common.Data;
using View4Logs.Core.Utils;

namespace View4Logs.Core.LogSources
{
    public sealed class Log4JXmlLogFileSource : XmlLogFileSourceBase
    {
        private const string Log4JNamespaceName = "http://logging.apache.org/log4j/2.0/events";
        private const string NLogNamespaceName = "http://www.nlog-project.org/schemas/dummy.xsd";

        private static readonly Dictionary<string, LogLevel> LogLevelMapping = new Dictionary<string, LogLevel>
        {
            { "TRACE", LogLevel.Trace },
            { "DEBUG", LogLevel.Debug },
            { "INFO", LogLevel.Info },
            { "WARN", LogLevel.Warn },
            { "ERROR", LogLevel.Error },
            { "FATAL", LogLevel.Fatal },
        };

        public Log4JXmlLogFileSource(string path)
            : base(path)
        {
        }

        protected override XName LogEventElementName { get; } = XName.Get("event", Log4JNamespaceName);

        protected override (string prefix, string uri)[] KnownNamespaces { get; } =
        {
            ("log4j", Log4JNamespaceName),
            ("nlog", NLogNamespaceName)
        };

        protected override LogEvent ConvertElementToLogEvent(XElement el)
        {
            var timestamp = long.Parse(el.Attribute(XName.Get("timestamp")).Value);

            var logEvent = new LogEvent
            {
                Source = this,
                Message = el.Element(XName.Get("message", Log4JNamespaceName)).Value,
                TimeStamp = UnixTimestampConverter.ConvertFromMilliseconds(timestamp),
                Level = LogLevelMapping[el.Attribute(XName.Get("level")).Value]
            };

            return logEvent;
        }
    }
}
