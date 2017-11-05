using System;
using System.Reactive.Linq;
using View4Logs.Common;

namespace View4Logs.Services
{
    public sealed class LogSourceService : ILogSourceService
    {
        public LogSourceService()
        {
            var rand = new Random();
            var messages = new[]
            {
                "Some information",
                "Catastrophic error",
                "Just a warning, ignore me",
                "TRACE: nothing important"
            };

            Messages = Observable.Interval(TimeSpan.FromSeconds(2)).Select(_ => new LogMessage { Message = messages[rand.Next(0, messages.Length -1)] });
        }

        public IObservable<LogMessage> Messages { get; }
    }
}
