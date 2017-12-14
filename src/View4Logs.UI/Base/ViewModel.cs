using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using System.Windows.Documents;

namespace View4Logs.UI.Base
{
    public abstract class ViewModel : INotifyPropertyChanged, IDisposable
    {
        private CompositeDisposable _disposables;

        public event PropertyChangedEventHandler PropertyChanged;

        private CompositeDisposable Disposables => _disposables ?? (_disposables = new CompositeDisposable());

        protected T SafeDispose<T>(T disposable) where T : IDisposable
        {
            Disposables.Add(disposable);
            return disposable;
        }

        protected void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool Set<T>(ref T field, T newValue, [CallerMemberName]string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(newValue, field))
            {
                return false;
            }

            field = newValue;
            RaisePropertyChanged(propertyName);
            return true;
        }

        protected ObservableProperty<T> CreateProperty<T>(string propertyName)
        {
            return SafeDispose(new ObservableProperty<T>(propertyName, RaisePropertyChanged));
        }

        protected ObservableProperty<T> CreateProperty<T>(string propertyName, T initialValue)
        {
            return SafeDispose(new ObservableProperty<T>(propertyName, RaisePropertyChanged, initialValue));
        }

        protected ObservableProperty<T> CreateProperty<T>(string propertyName, IObservable<T> source)
        {
            var result = SafeDispose(new ObservableProperty<T>(propertyName, RaisePropertyChanged));
            SafeDispose(source.Subscribe(result));
            return result;
        }

        protected ObservableProperty<T> CreateProperty<T>(string propertyName, T initialValue, IObservable<T> source)
        {
            var result = SafeDispose(new ObservableProperty<T>(propertyName, RaisePropertyChanged, initialValue));
            SafeDispose(source.Subscribe(result));
            return result;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _disposables?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
