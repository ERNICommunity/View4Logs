using System;

namespace View4Logs.Common
{
    public interface ILogSource
    {
        IObservable<LogMessage> Messages { get; }
    }
}
