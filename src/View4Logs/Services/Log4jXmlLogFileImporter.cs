using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;
using View4Logs.Utils;
using View4Logs.Utils.Xml;

namespace View4Logs.Services
{
    public sealed class Log4JXmlLogFileImporter : ILogFileImporter
    {
        private const string Log4jNS = "http://logging.apache.org/log4j/2.0/events";
        private const string NLogNS = "http://www.nlog-project.org/schemas/NLog.xsd";

        private readonly ILogSourceService _logSourceService;

        private readonly Dictionary<string, LogLevel> _logLevelMapping = new Dictionary<string, LogLevel>
        {
            { "TRACE", LogLevel.Trace },
            { "DEBUG", LogLevel.Debug },
            { "INFO", LogLevel.Info },
            { "WARN", LogLevel.Warn },
            { "ERROR", LogLevel.Error },
            { "FATAL", LogLevel.Fatal },
        };

        public Log4JXmlLogFileImporter(ILogSourceService logSourceService)
        {
            _logSourceService = logSourceService;
        }

        public void Import(string filename)
        {
            var source = new FileLogSource(filename, ProcessStream);
            _logSourceService.Clear();
            _logSourceService.AddSource(source);
            source.Start();
        }

        private IEnumerable<LogMessage> ProcessStream(Stream stream)
        {
            var xmlFragmentReader = new XmlFragmentReader(stream);
            var ns = xmlFragmentReader.NamespaceManager;
            ns.AddNamespace("log4j", Log4jNS);
            ns.AddNamespace("nlog", NLogNS);

            foreach (var el in xmlFragmentReader.Read())
            {
                var timestamp = long.Parse(el.Attribute(XName.Get("timestamp")).Value);

                var logMessage = new LogMessage
                {
                    Message = el.Element(XName.Get("message", Log4jNS)).Value,
                    TimeStamp = UnixTimestampConverter.ConvertFromMilliseconds(timestamp),
                    Level = _logLevelMapping[el.Attribute(XName.Get("level")).Value]
                };

                yield return logMessage;
            }
        }
    }
}
