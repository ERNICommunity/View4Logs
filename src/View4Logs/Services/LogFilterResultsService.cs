using System;
using System.Linq;
using System.Reactive.Linq;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;

namespace View4Logs.Services
{
    public sealed class LogFilterResultsService : ILogFilterResultsService
    {
        public LogFilterResultsService(ILogSourceService logSourceService, ILogFilterService logFilterService)
        {
            // Produces new array of messages whenever arrives new message which passes the filter or the filter is changed.
            Messages = Observable.CombineLatest(
                logFilterService.Filter,
                logSourceService.Messages,
                FilterMessages
            ).Switch();
        }

        public IObservable<LogMessage[]> Messages { get; }

        private IObservable<LogMessage[]> FilterMessages(Func<LogMessage, bool> filter, ILogMessagesObservableBuffer buf)
        {
            // Internal buffer for messages which passes the filter condition.
            var filteredMessages = buf.Messages.Where(filter).ToList();

            // Return observable sequence beginning with the current filtered messages.
            // Produce new value for every message which passes the filter.
            return Observable.Concat(
                Observable.Return(filteredMessages.ToArray()),
                buf.NewMessages.Where(filter).Select(msg =>
                {
                    // Add log message to internal buffer.
                    filteredMessages.Add(msg);
                    // Return copy of the internal buffer to avoid threading issues and external modifications.
                    return filteredMessages.ToArray();
                })
            );
        }
    }
}