using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace View4Logs.Utils.Collections
{
    public static class NotifyListChangedExtensions
    {
        /// <summary>
        /// Creates observable sequence from <paramref name="source"/> ListChanged events.
        /// </summary>
        public static IObservable<NotifyListChangedEventArgs<T>> AsObservable<T>(this INotifyListChanged<T> source)
        {
            return Observable.FromEventPattern<NotifyListChangedEventArgs<T>>(
                h => source.ListChanged += h,
                h => source.ListChanged -= h
            ).Select(e => e.EventArgs);
        }

        /// <summary>
        /// Creates observable sequence from <paramref name="source"/> ListChanged events.
        /// Observable will start with <see cref="NotifyListChangedAction.Reset"/> notification containing current snapshot of items.
        /// </summary>        
        public static IObservable<NotifyListChangedEventArgs<T>> AsBehaviorObservable<T>(this INotifyListChanged<T> source)
        {
            return Observable.Defer(() =>
                {
                    var items = source.GetSnapshot();
                    return Observable.Return(new NotifyListChangedEventArgs<T>(NotifyListChangedAction.Reset, items, items));
                })
                .Concat(
                    source.AsObservable()
                );
        }

        /// <summary>
        /// Creates observable sequence from <paramref name="source"/> ListChanged events and selects the current snapshot of the list (<see cref="NotifyListChangedEventArgs{T}.Items"/>).
        /// </summary>        
        public static IObservable<IList<T>> AsItemsBehaviorObservable<T>(this INotifyListChanged<T> source)
        {
            return source.AsBehaviorObservable().Select(e => e.Items);
        }
    }
}
