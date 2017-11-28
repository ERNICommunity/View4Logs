using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using View4Logs.Common.Data;
using View4Logs.Utils;
using View4Logs.Utils.IO;

namespace View4Logs.LogSources
{
    public sealed class Log4JXmlLogFileSource : LogFileSourceBase
    {
        private const string Log4JNamespaceName = "http://logging.apache.org/log4j/2.0/events";
        private const string NLogNamespaceName = "http://www.nlog-project.org/schemas/NLog.xsd";

        private static readonly string XmlPrefix = $"<root xmlns:log4j=\"{Log4JNamespaceName}\" xmlns:nlog=\"{NLogNamespaceName}\">";
        private static readonly string XmlSufix = "</root>";

        private static readonly Dictionary<string, LogLevel> LogLevelMapping = new Dictionary<string, LogLevel>
        {
            { "TRACE", LogLevel.Trace },
            { "DEBUG", LogLevel.Debug },
            { "INFO", LogLevel.Info },
            { "WARN", LogLevel.Warn },
            { "ERROR", LogLevel.Error },
            { "FATAL", LogLevel.Fatal },
        };

        private static readonly XmlReaderSettings _readerSettings = new XmlReaderSettings
        {
            IgnoreComments = true,
            IgnoreProcessingInstructions = true,
            IgnoreWhitespace = true,
            CloseInput = false
        };

        public Log4JXmlLogFileSource(string path)
            : base(path)
        {

        }

        protected override IList<LogMessage> ProcessStream(FileStream stream)
        {
            var eventName = XName.Get("event", Log4JNamespaceName);
            var messages = new List<LogMessage>();

            // Log file is actually not well formed XML document.
            // There is no root element and XML namespace prefixes are used without definition.
            // Therefore we wrap the whole content by "fake" root element with needed prefix declarations.
            using (var textReader = new ConcatTextReader(new StringReader(XmlPrefix), new StreamReader(stream, Encoding.Default, true, 1024, true), new StringReader(XmlSufix)))
            using (var xmlReader = XmlReader.Create(textReader, _readerSettings))
            {
                xmlReader.MoveToContent();
                xmlReader.Read();

                while (xmlReader.NodeType != XmlNodeType.EndElement)
                {
                    var node = XNode.ReadFrom(xmlReader);
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        var el = (XElement)node;
                        if (el.Name == eventName)
                        {
                            var msg = ConvertElementToLogMessage(el);
                            messages.Add(msg);
                        }
                    }
                }
            }

            return messages;
        }

        private LogMessage ConvertElementToLogMessage(XElement el)
        {
            var timestamp = long.Parse(el.Attribute(XName.Get("timestamp")).Value);

            var logMessage = new LogMessage
            {
                Source = this,
                Message = el.Element(XName.Get("message", Log4JNamespaceName)).Value,
                TimeStamp = UnixTimestampConverter.ConvertFromMilliseconds(timestamp),
                Level = LogLevelMapping[el.Attribute(XName.Get("level")).Value]
            };

            return logMessage;
        }
    }
}