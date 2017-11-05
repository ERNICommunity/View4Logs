using System;
using View4Logs.Common;

namespace View4Logs.Services
{
    public interface ILogFilterService
    {
        IObservable<Func<LogMessage, bool>> Filter { get; }
    }
}
