using System;
using View4Logs.Common;

namespace View4Logs.Services
{
    public class LogFilterService : ILogFilterService
    {
        public IObservable<Func<LogMessage, bool>> Filter { get; }
    }
}
