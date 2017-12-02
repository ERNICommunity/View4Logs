using System;
using System.Collections.Generic;
using System.Xml.Linq;
using View4Logs.Common.Data;

namespace View4Logs.LogSources
{
    public sealed class Log4NetXmlLogFileSource : XmlLogFileSourceBase
    {
        private const string Log4netNamespaceName = "http://logging.apache.orglog4net/schemaslog4net-events-1.2";

        private static readonly Dictionary<string, LogLevel> LogLevelMapping = new Dictionary<string, LogLevel>
        {
            { "DEBUG", LogLevel.Debug },
            { "INFO", LogLevel.Info },
            { "WARN", LogLevel.Warn },
            { "ERROR", LogLevel.Error },
            { "FATAL", LogLevel.Fatal },
        };

        public Log4NetXmlLogFileSource(string path)
            : base(path)
        {
        }

        protected override XName LogMessageElementName { get; } = XName.Get("event", Log4netNamespaceName);

        protected override (string prefix, string uri)[] KnownNamespaces { get; } =
        {
            ("log4net", Log4netNamespaceName),
        };

        protected override LogMessage ConvertElementToLogMessage(XElement el)
        {
            var logMessage = new LogMessage
            {
                Source = this,
                Message = el.Element(XName.Get("message", Log4netNamespaceName)).Value,
                TimeStamp = DateTime.Parse(el.Attribute(XName.Get("timestamp")).Value),
                Level = LogLevelMapping[el.Attribute(XName.Get("level")).Value]
            };

            return logMessage;
        }
    }
}
