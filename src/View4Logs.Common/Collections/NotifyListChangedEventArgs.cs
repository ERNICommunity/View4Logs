using System;
using System.Collections.Generic;

namespace View4Logs.Common.Collections
{
    /// <summary>
    /// Provides data for the <see cref="INotifyListChanged{T}.ListChanged"/> event.
    /// </summary>
    public sealed class NotifyListChangedEventArgs<T> : EventArgs
    {
        public NotifyListChangedEventArgs(NotifyListChangedAction action, IList<T> items, IList<T> newItems)
        {
            Action = action;
            Items = items;
            NewItems = newItems;
        }

        /// <summary>
        /// Gets the action that caused the event.
        /// </summary>
        public NotifyListChangedAction Action { get; }

        /// <summary>
        /// Current snapshot of items in the list
        /// </summary>
        public IList<T> Items { get; }

        /// <summary>
        /// Snapshot of items added to the list.
        /// In case of <see cref="NotifyListChangedAction.Reset"/> action, it will contain all items (effectively same object as <see cref="Items"/>).
        /// </summary>
        public IList<T> NewItems { get; }
    }
}
