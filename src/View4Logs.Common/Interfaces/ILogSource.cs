using System;
using System.Collections.Generic;
using View4Logs.Common.Data;

namespace View4Logs.Common.Interfaces
{
    public interface ILogSource : IDisposable
    {
        /// <summary>
        /// User friendly name of the source.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// User friendly identifier of the source (e.g. file absolute path).
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Start processing input. Before this call, no log event is streamed via <see cref="LogEvents"/> property.
        /// </summary>
        void Start();

        /// <summary>
        /// Buffered stream of log events.
        /// </summary>
        IObservable<IList<LogEvent>> LogEvents { get; }

        /// <summary>
        /// Signals when all previous log events from this log source should be removed.
        /// </summary>
        IObservable<ILogSource> Reset { get; }
    }
}
