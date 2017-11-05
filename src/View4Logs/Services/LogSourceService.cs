using System;
using System.Reactive.Linq;
using View4Logs.Common;

namespace View4Logs.Services
{
    public sealed class LogSourceService : ILogSourceService
    {
        public LogSourceService()
        {
            Messages = Observable.Interval(TimeSpan.FromSeconds(2)).Select(_ => new LogMessage { Message = "Lorem Ipsum" });
        }

        public IObservable<LogMessage> Messages { get; }
    }
}
