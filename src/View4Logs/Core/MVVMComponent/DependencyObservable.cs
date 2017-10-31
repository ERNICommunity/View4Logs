using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Windows;

namespace View4Logs.Core.MVVMComponent
{
    public static class DependencyObservable
    {
        public static IObservable<T> PropertyAsObservable<T>(this DependencyObject target, DependencyProperty property)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            return Observable.Create<T>(o =>
            {
                var d = DependencyPropertyDescriptor.FromProperty(property, target.GetType());
                EventHandler handler = (s, e) => o.OnNext((T)target.GetValue(property));
                d.AddValueChanged(target, handler);
                return () => d.RemoveValueChanged(target, handler);
            });
        }
    }
}
