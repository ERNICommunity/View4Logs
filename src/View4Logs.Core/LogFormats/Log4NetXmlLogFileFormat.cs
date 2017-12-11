using System;
using System.Xml.Linq;
using View4Logs.Common.Interfaces;
using View4Logs.Core.LogSources;

namespace View4Logs.Core.LogFormats
{
    public sealed class Log4NetXmlLogFileFormat : XmlLogFileFormatBase
    {
        private const string Log4netNamespaceName = "http://logging.apache.orglog4net/schemaslog4net-events-1.2";

        public override string Name => "Log4net XML";

        public override ILogSource CreateSource(Uri uri)
        {
            return new Log4NetXmlLogFileSource(uri.LocalPath);
        }

        protected override (string prefix, string uri)[] KnownNamespaces { get; } =
        {
            ("log4net", Log4netNamespaceName),
        };

        protected override bool CheckElement(XElement element)
        {
            return element.Name.LocalName == "event" && element.Name.Namespace == Log4netNamespaceName;
        }
    }
}
