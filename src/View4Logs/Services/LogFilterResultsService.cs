using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
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
                logFilterService.Filter.ObserveOn(TaskPoolScheduler.Default),
                logSourceService.Messages.ObserveOn(TaskPoolScheduler.Default),
                FilterMessages
            ).Switch();
        }

        public IObservable<IList<LogMessage>> Messages { get; }

        private IObservable<IList<LogMessage>> FilterMessages(Func<LogMessage, bool> filter, ILogMessagesObservableBuffer buf)
        {
            // Internal buffer for messages which passes the filter condition.
            var filteredMessagesBuffer = new AppendBuffer<LogMessage>(BufferDefaultSize);
            
            return Observable.Concat(
                // Filter all current messages asynchronously with cancellation support
                Observable.StartAsync(async token =>
                {
                    await Task.Run(() => filteredMessagesBuffer.AddRange(ApplyFilter(buf.Messages, filter, token)), token);
                    return filteredMessagesBuffer.Snapshot();
                }),
                // Append all following messages which passes the filter
                buf.NewMessages.Where(filter).Select(msg =>
                {
                    filteredMessagesBuffer.Add(msg);
                    return filteredMessagesBuffer.Snapshot();
                })
            );
        }

        private IEnumerable<LogMessage> ApplyFilter(IReadOnlyList<LogMessage> messages, Func<LogMessage, bool> filter, CancellationToken token)
        {            
            return messages
                .AsParallel()
                .AsOrdered()
                .WithCancellation(token)
                .Where(filter);
        }
    }
}