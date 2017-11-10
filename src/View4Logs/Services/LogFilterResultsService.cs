using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;
using View4Logs.Utils;

namespace View4Logs.Services
{
    public sealed class LogFilterResultsService : ILogFilterResultsService
    {
        private const int BufferDefaultSize = 1024;

        public LogFilterResultsService(ILogSourceService logSourceService, ILogFilterService logFilterService)
        {
            // Produces new array of messages whenever arrives new message which passes the filter or the filter is changed.
            Messages = Observable.CombineLatest(
                logFilterService.Filter,
                logSourceService.Messages,
                FilterMessages
            ).Switch();
        }

        public IObservable<IList<LogMessage>> Messages { get; }

        private IObservable<IList<LogMessage>> FilterMessages(Func<LogMessage, bool> filter, ILogMessagesObservableBuffer buf)
        {
            // Internal buffer for messages which passes the filter condition.
            var filteredMessagesBuffer = new AppendBuffer<LogMessage>(BufferDefaultSize);
            filteredMessagesBuffer.AddRange(buf.Messages.Where(filter));

            // Return observable sequence beginning with the current filtered messages.
            // Produce new value for every message which passes the filter.
            return Observable.Concat(
                Observable.Return(filteredMessagesBuffer.Snapshot()),
                buf.NewMessages.Where(filter).Select(msg =>
                {
                    // Add log message to internal buffer.
                    filteredMessagesBuffer.Add(msg);
                    // Return copy of the internal buffer to avoid threading issues and external modifications.
                    return filteredMessagesBuffer.Snapshot();
                })
            );
        }
    }
}