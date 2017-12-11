using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace View4Logs.Core.LogFormats
{
    public abstract class XmlLogFileFormatBase : LogFileFormatBase
    {
        protected abstract (string prefix, string uri)[] KnownNamespaces { get; }

        protected override bool CheckFilename(string path)
        {
            var ext = Path.GetExtension(path).ToLower();
            if (string.IsNullOrEmpty(ext) || ext == ".xml" || ext == ".log")
            {
                return true;
            }

            return false;
        }

        protected override bool CheckContent(FileStream stream)
        {
            var nsManager = new XmlNamespaceManager(new NameTable());
            foreach ((var prefix, var uri) in KnownNamespaces)
            {
                nsManager.AddNamespace(prefix, uri);
            }

            var readerSettings = new XmlReaderSettings
            {
                ConformanceLevel = ConformanceLevel.Fragment,
                IgnoreComments = true,
                IgnoreProcessingInstructions = true,
                IgnoreWhitespace = true,
                NameTable = nsManager.NameTable
            };

            var parserContext = new XmlParserContext(null, nsManager, "en", XmlSpace.None);

            using (var textReader = new StreamReader(stream, Encoding.Default, true, 1024, true))
            using (var xmlReader = XmlReader.Create(textReader, readerSettings, parserContext))
            {
                xmlReader.MoveToContent();

                if (xmlReader.NodeType != XmlNodeType.None)
                {
                    var node = XNode.ReadFrom(xmlReader);
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        var el = (XElement)node;
                        if (!CheckElement(el))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        protected abstract bool CheckElement(XElement element);
    }
}
