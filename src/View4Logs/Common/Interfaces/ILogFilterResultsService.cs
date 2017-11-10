using System;
using View4Logs.Common.Data;

namespace View4Logs.Common.Interfaces
{
    public interface ILogFilterResultsService
    {
        IObservable<LogMessage[]> Messages { get; }
    }
}