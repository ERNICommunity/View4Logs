using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using View4Logs.Utils.Streams;

namespace View4Logs.Utils.Xnl
{
    public sealed class XmlFragmentReader
    {
        private readonly Stream _stream;

        public XmlFragmentReader(Stream stream)
        {
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
            NamespaceManager = new XmlNamespaceManager(new NameTable());
        }

        public XmlNamespaceManager NamespaceManager { get; }

        public IEnumerable<XElement> Read()
        {            
            using (var prefixWriter = new StreamWriter(new MemoryStream(), Encoding.UTF8))
            using (var sufixWriter = new StreamWriter(new MemoryStream(), Encoding.UTF8))
            {
                prefixWriter.Write("<root ");

                var namespaces = NamespaceManager.GetNamespacesInScope(XmlNamespaceScope.All);
                foreach (var kv in namespaces)
                {
                    prefixWriter.Write($"xmlns:{kv.Key}=\"{kv.Value}\" ");
                }

                prefixWriter.Write('>');
                sufixWriter.Write("</root>");

                prefixWriter.Flush();
                sufixWriter.Flush();

                var prefixStream = prefixWriter.BaseStream;
                var sufixStream = sufixWriter.BaseStream;

                prefixStream.Position = 0;
                sufixStream.Position = 0;

                var xmlStream = prefixStream.Concat(_stream).Concat(sufixStream);
                using (var xmlReader = XmlReader.Create(xmlStream, new XmlReaderSettings { NameTable = NamespaceManager.NameTable, IgnoreComments = true, IgnoreProcessingInstructions = true, IgnoreWhitespace = true }))
                {
                    // Read the "<root>" element
                    xmlReader.ReadStartElement();

                    XElement el;
                    while ((el = XNode.ReadFrom(xmlReader) as XElement) != null)
                    {
                        yield return el;
                    }
                }
            }                
        }
    }
}