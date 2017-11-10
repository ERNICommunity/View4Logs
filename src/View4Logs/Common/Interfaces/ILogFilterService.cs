using System;
using View4Logs.Common.Data;

namespace View4Logs.Common.Interfaces
{
    public interface ILogFilterService
    {
        IObservable<Func<LogMessage, bool>> Filter { get; }

        void AddFilter(IObservable<Func<LogMessage, bool>> filter);
    }
}
