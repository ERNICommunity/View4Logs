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
        /// Start processing input. Before this call, no message is streamed via <see cref="Messages"/> property.
        /// </summary>
        void Start();

        /// <summary>
        /// Buffered stream of log messages.
        /// </summary>
        IObservable<IList<LogMessage>> Messages { get; }

        /// <summary>
        /// Signals when all previous messages from this log source should be removed.
        /// </summary>
        IObservable<ILogSource> Reset { get; }
    }
}
