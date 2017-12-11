using System;
using System.Xml.Linq;
using View4Logs.Common.Interfaces;
using View4Logs.Core.LogSources;

namespace View4Logs.Core.LogFormats
{
    public sealed class Log4JXmlLogFileFormat : XmlLogFileFormatBase
    {
        private const string Log4JNamespaceName = "http://logging.apache.org/log4j/2.0/events";
        private const string NLogNamespaceName = "http://www.nlog-project.org/schemas/dummy.xsd";

        public override string Name => "Log4j compatible XML";

        public override ILogSource CreateSource(Uri uri)
        {
            return new Log4JXmlLogFileSource(uri.LocalPath);
        }

        protected override (string prefix, string uri)[] KnownNamespaces { get; } =
        {
            ("log4j", Log4JNamespaceName),
            ("nlog", NLogNamespaceName)
        };

        protected override bool CheckElement(XElement element)
        {
            return element.Name.LocalName == "event" && element.Name.Namespace == Log4JNamespaceName;
        }
    }
}
