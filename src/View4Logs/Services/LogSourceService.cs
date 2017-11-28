using System;
using System.Linq;
using System.Collections.Generic;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;
using View4Logs.Utils.Collections;

namespace View4Logs.Services
{
    public sealed class LogSourceService : ILogSourceService, IDisposable
    {
        private readonly object _messagesLock;
        private readonly object _sourcesLock;
        private readonly ObservableCowList<ILogSource> _sources;
        private readonly ObservableCowList<LogMessage> _messages;

        public LogSourceService()
        {
            _messagesLock = new object();
            _sourcesLock = new object();
            _sources = new ObservableCowList<ILogSource>();
            _messages = new ObservableCowList<LogMessage>();

            Sources = _sources;
            Messages = _messages;
        }

        public INotifyListChanged<LogMessage> Messages { get; }

        public INotifyListChanged<ILogSource> Sources { get; }

        public void AddSource(ILogSource source)
        {
            lock (_sourcesLock)
            {
                _sources.Add(source);

                source.Messages.Subscribe(
                    Append,
                    () => _sources.Remove(source)
                );

                source.Reset.Subscribe(ResetSource);
            }
        }

        public void ResetSource(ILogSource source)
        {
            lock (_messagesLock)
            {
                var messages = _messages.Where(msg => msg.Source != source).ToList();
                _messages.Reset(messages);
            }
        }

        private void Append(IList<LogMessage> messages)
        {
            lock (_messagesLock)
            {
                var needSort = messages.Zip(messages.Skip(1), (a, b) => a.TimeStamp < b.TimeStamp).Contains(false);
                if (needSort)
                {
                    messages = messages.OrderBy(msg => msg.TimeStamp).ToList();
                }

                _messages.Add(messages);
            }
        }

        public void Clear()
        {
            lock (_sourcesLock)
            {
                lock (_messagesLock)
                {
                    foreach (var src in _sources)
                    {
                        src.Dispose();
                    }

                    _sources.Clear();
                    _messages.Clear();
                }
            }
        }

        public void Dispose()
        {
            Clear();
        }
    }
}
