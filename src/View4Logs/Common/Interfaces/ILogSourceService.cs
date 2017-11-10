using System;
using System.Collections.Generic;
using View4Logs.Common.Data;

namespace View4Logs.Common.Interfaces
{
    public interface ILogSourceService
    {
        IObservable<ILogMessagesObservableBuffer> Messages { get; }

        void Append(LogMessage message);

        void Reset(List<LogMessage> messages);
    }
}
