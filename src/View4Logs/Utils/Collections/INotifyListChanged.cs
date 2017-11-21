using System;
using System.Collections.Generic;

namespace View4Logs.Utils.Collections
{
    /// <summary>
    /// Notifies listeners when list is changed.    
    /// </summary>
    /// <remarks>
    /// It has similar purpose as INotifyCollectionChanged, but is simplified and specialized for performance & (thread) safety requirements.
    /// List can notify that items has been added to the end of the list, or the whole list is reset.
    /// Notification objects contains readonly snapshot of added items and also current snapshot of entire list.
    /// Since the snapshot are "immutable", they can be *safely* shared between threads without additional synchronization.
    /// </remarks>
    public interface INotifyListChanged<T>
    {
        /// <summary>
        /// Notifies listeners when list is changed.
        /// </summary>
        event EventHandler<NotifyListChangedEventArgs<T>> ListChanged;

        /// <summary>
        /// Returns snapshot of current items as readonly list.
        /// </summary>
        /// <remarks>
        /// Snapshot is independent from source list and can be shared between threads without additional synchronization.
        /// Be aware that while arrays (backing store for snapshot items) reports them selfs as readonly, they will not throw when modified.
        /// </remarks>
        IList<T> GetSnapshot();
    }
}
