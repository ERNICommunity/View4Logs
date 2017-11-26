using System;
using System.Reactive.Subjects;
using System.Xml.Linq;
using Sax.Net;

namespace View4Logs.Utils.Xml
{
    /// <summary>
    /// Sax content handler which emits requested elements as <see cref="XElement"/> through observable sequence.
    /// </summary>
    /// <remarks>
    /// Purpose of this class is to provide observable sequence (stream) of requested elements.
    /// It was needed because standard <see cref="System.Xml.XmlReader"/> did not work well together with <see cref="View4Logs.Utils.Streams.BlockingRetryStream"/>
    /// </remarks>
    public sealed class SaxToXElementHandler : IContentHandler
    {
        private readonly XName _elementName;
        private readonly Subject<XElement> _elements;
        private XElement _current;

        public SaxToXElementHandler(XName elementName)
        {
            _elementName = elementName;
            _elements = new Subject<XElement>();
        }

        public IObservable<XElement> Elements => _elements;

        void IContentHandler.SetDocumentLocator(ILocator locator)
        {
            // Do nothing
        }

        void IContentHandler.StartDocument()
        {
            // Do nothing
        }

        void IContentHandler.EndDocument()
        {
            // Do nothing
        }

        void IContentHandler.StartPrefixMapping(string prefix, string uri)
        {
            // Do nothing
        }

        void IContentHandler.EndPrefixMapping(string prefix)
        {
            // Do nothing
        }

        void IContentHandler.StartElement(string uri, string localName, string qName, IAttributes atts)
        {
            var name = XName.Get(localName, uri);

            if (_current != null || name == _elementName)
            {
                var element = new XElement(name);

                for (int i = 0, l = atts.Length; i < l; i++)
                {
                    var attrLocalName = atts.GetLocalName(i);
                    var attrUri = atts.GetUri(i);
                    var attrValue = atts.GetValue(i);

                    element.SetAttributeValue(XName.Get(attrLocalName, attrUri), attrValue);
                }

                _current?.Add(element);
                _current = element;
            }
        }

        void IContentHandler.EndElement(string uri, string localName, string qName)
        {
            if (_current.Parent == null)
            {
                // It's a root element in which user is interested.
                _elements.OnNext(_current);
            }
            
            _current = _current.Parent;
        }

        void IContentHandler.Characters(char[] ch, int start, int length)
        {
            _current?.Add(new string(ch, start, length));
        }

        void IContentHandler.IgnorableWhitespace(char[] ch, int start, int length)
        {
            // Do nothing
        }

        void IContentHandler.ProcessingInstruction(string target, string data)
        {
            // Do nothing
        }

        void IContentHandler.SkippedEntity(string name)
        {
            // Do nothing
        }
    }
}