using System;
using System.Collections.Generic;
using View4Logs.Common.Data;

namespace View4Logs.Common.Interfaces
{
    public interface ILogMessagesObservableBuffer
    {
        IReadOnlyList<LogMessage> Messages { get; }

        IObservable<LogMessage> NewMessages { get; }
    }
}