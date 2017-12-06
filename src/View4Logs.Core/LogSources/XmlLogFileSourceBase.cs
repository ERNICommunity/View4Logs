using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using View4Logs.Common.Data;

namespace View4Logs.Core.LogSources
{
    public abstract class XmlLogFileSourceBase : LogFileSourceBase
    {
        private XmlReaderSettings _readerSettings;
        private XmlParserContext _parserContext;

        protected XmlLogFileSourceBase(string path)
            : base(path)
        { }

        protected abstract XName LogMessageElementName { get; }

        protected abstract (string prefix, string uri)[] KnownNamespaces { get; }

        protected override void Initialize()
        {
            base.Initialize();

            var nsManager = new XmlNamespaceManager(new NameTable());
            foreach ((var prefix, var uri) in KnownNamespaces)
            {
                nsManager.AddNamespace(prefix, uri);
            }

            _readerSettings = new XmlReaderSettings
            {
                ConformanceLevel = ConformanceLevel.Fragment,
                IgnoreComments = true,
                IgnoreProcessingInstructions = true,
                IgnoreWhitespace = true,
                NameTable = nsManager.NameTable
            };

            _parserContext = new XmlParserContext(null, nsManager, "en", XmlSpace.None);
        }

        // Log file is actually not well formed XML document.
        // There is no root element and XML namespace prefixes are used without definition.
        // Therefore we have to treat it as a stream of XML fragments and provide required contextual information
        protected sealed override IList<LogMessage> ProcessStream(FileStream stream)
        {
            var result = new List<LogMessage>();

            using (var textReader = new StreamReader(stream, Encoding.Default, true, 1024, true))
            using (var xmlReader = XmlReader.Create(textReader, _readerSettings, _parserContext))
            {
                xmlReader.MoveToContent();

                while (xmlReader.NodeType != XmlNodeType.None)
                {
                    var node = XNode.ReadFrom(xmlReader);
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        var el = (XElement)node;
                        if (el.Name == LogMessageElementName)
                        {
                            var msg = ConvertElementToLogMessage(el);
                            result.Add(msg);
                        }
                    }
                }
            }

            return result;
        }

        protected abstract LogMessage ConvertElementToLogMessage(XElement el);
    }
}
