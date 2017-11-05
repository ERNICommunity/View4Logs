using System;
using View4Logs.Common;

namespace View4Logs.Services
{
    public interface ILogSourceService
    {
        IObservable<LogMessage> Messages { get; }
    }
}
