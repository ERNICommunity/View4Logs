using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;

namespace View4Logs.Services
{
    public sealed class LogSourceService : ILogSourceService
    {
        private readonly object _messagesLock;
        private readonly BehaviorSubject<LogMessagesObservableBuffer> _messages;

        public LogSourceService()
        {
            _messagesLock = new object();
            _messages = new BehaviorSubject<LogMessagesObservableBuffer>(new LogMessagesObservableBuffer());
            Messages = _messages.AsObservable();
        }

        public IObservable<ILogMessagesObservableBuffer> Messages { get; }

        public void Append(LogMessage message)
        {
            lock (_messagesLock)
            {
                _messages.Value.Append(message);
            }
        }

        public void Reset(List<LogMessage> messages)
        {
            lock (_messagesLock)
            {
                _messages.Value.Dispose();
                _messages.OnNext(new LogMessagesObservableBuffer(messages));
            }
        }

        private sealed class LogMessagesObservableBuffer : ILogMessagesObservableBuffer, IDisposable
        {
            private readonly List<LogMessage> _messages;
            private readonly Subject<LogMessage> _newMessages;

            public LogMessagesObservableBuffer()
                : this(new List<LogMessage>())
            { }

            public LogMessagesObservableBuffer(List<LogMessage> messages)
            {
                _messages = messages;
                _newMessages = new Subject<LogMessage>();
                NewMessages = _newMessages.AsObservable();
            }

            public IReadOnlyList<LogMessage> Messages => _messages;

            public IObservable<LogMessage> NewMessages { get; }

            public void Append(LogMessage message)
            {
                _messages.Add(message);
                _newMessages.OnNext(message);
            }

            public void Dispose()
            {
                _newMessages.Dispose();
            }
        }
    }
}
