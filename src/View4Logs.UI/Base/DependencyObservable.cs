using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;

namespace View4Logs.UI.Base
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

        public static IDisposable Bind<T>(this DependencyObject target, DependencyProperty dp, IObservable<T> source)
        {
            return source.Subscribe(value => target.SetValue(dp, value));
        }

        public static IDisposable Bind<T>(this DependencyObject target, DependencyPropertyKey dpKey, IObservable<T> source)
        {
            return source.Subscribe(value => target.SetValue(dpKey, value));
        }

        public static IDisposable BindTwoWay<T>(this DependencyObject target, DependencyProperty dp, ISubject<T> source)
        {
            var d1 = target.PropertyAsObservable<T>(dp).Subscribe(source);
            var d2 = source.Subscribe(value => target.SetValue(dp, value));
            return new CompositeDisposable(d1, d2);
        }

        public static IDisposable BindTwoWay<T>(this DependencyObject target, ISubject<T> source, DependencyPropertyKey dpKey)
        {
            var d1 = target.PropertyAsObservable<T>(dpKey.DependencyProperty).Subscribe(source);
            var d2 = source.Subscribe(value => target.SetValue(dpKey, value));
            return new CompositeDisposable(d1, d2);
        }
    }
}
