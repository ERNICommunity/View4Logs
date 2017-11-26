using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Xml.Linq;
using AElfred.Net;
using Sax.Net;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;
using View4Logs.Utils;
using View4Logs.Utils.Streams;
using View4Logs.Utils.Xml;

namespace View4Logs.LogSources
{
    public sealed class Log4JXmlLogFileSource : ILogSource
    {
        private const string Log4JNamespaceName = "http://logging.apache.org/log4j/2.0/events";
        private const string NLogNamespaceName = "http://www.nlog-project.org/schemas/NLog.xsd";
        private const int ReadRetryDelay = 100;

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

        private readonly Subject<IList<LogMessage>> _messages;
        private readonly CancellationTokenSource _cts;
        private readonly Thread _processingThread;
        private bool _started;
        private bool _disposed;

        public Log4JXmlLogFileSource(string path)
        {
            Name = Path.GetFileNameWithoutExtension(path);
            FullPath = Path.GetFullPath(path);

            _messages = new Subject<IList<LogMessage>>();
            _cts = new CancellationTokenSource();

            _processingThread = new Thread(ProcessFile);
        }

        public string Name { get; }

        public string FullPath { get; }

        public void Start()
        {
            ThrowIfDisposed();

            if (_started)
            {
                throw new InvalidOperationException("File log source can be started only once.");
            }

            _started = true;
            _processingThread.Start();
        }

        public IDisposable Subscribe(IObserver<IList<LogMessage>> observer)
        {
            ThrowIfDisposed();
            return _messages.Subscribe(observer);
        }

        private void ProcessFile()
        {
            FileStream OpenFile() => new FileStream(FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (var stream = new BlockingRetryStream(OpenFile(), ReadRetryDelay, _cts.Token))
            {
                var closingBoundaries = Observable.FromEventPattern(
                    h => stream.EndOfStreamReached += h,
                    h => stream.EndOfStreamReached -= h
                );

                ProcessStream(stream, closingBoundaries);
            }
        }

        private void ProcessStream(BlockingRetryStream stream, IObservable<object> bufferClosingBoundaries)
        {
            var eventName = XName.Get("event", Log4JNamespaceName);
            var factory = new XmlReaderFactory();
            var xmlReader = factory.CreateXmlReader();
            var handler = new SaxToXElementHandler(eventName);

            xmlReader.ContentHandler = handler;

            using (var textReader = new ConcatTextReader(new StringReader(XmlPrefix), new StreamReader(stream, true), new StringReader(XmlSufix)))
            {
                handler
                    .Elements
                    .Select(ConvertElementToLogMessage)
                    .Buffer(bufferClosingBoundaries)
                    .Subscribe(_messages);

                var input = new InputSource(textReader);
                xmlReader.Parse(input);
            }
        }

        private LogMessage ConvertElementToLogMessage(XElement el)
        {
            var timestamp = long.Parse(el.Attribute(XName.Get("timestamp")).Value);

            var logMessage = new LogMessage
            {
                Message = el.Element(XName.Get("message", Log4JNamespaceName)).Value,
                TimeStamp = UnixTimestampConverter.ConvertFromMilliseconds(timestamp),
                Level = LogLevelMapping[el.Attribute(XName.Get("level")).Value]
            };

            return logMessage;
        }

        public void Dispose()
        {
            _disposed = true;
            _cts.Cancel();
            _processingThread?.Join();
            _messages.Dispose();
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }
    }
}