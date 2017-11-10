using System;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace View4Logs.Utils
{
    public sealed class ObservableProperty<T> : ISubject<T>, IDisposable
    {
        private readonly Action<string> _raisePropertyChanged;
        private BehaviorSubject<T> _subject;

        public ObservableProperty(string propertyName, Action<string> raisePropertyChanged)
        {
            _raisePropertyChanged = raisePropertyChanged ?? throw new ArgumentNullException(nameof(raisePropertyChanged));
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            _subject = new BehaviorSubject<T>(default(T));
        }

        public ObservableProperty(string propertyName, Action<string> raisePropertyChanged, T initialValue)
        {
            _raisePropertyChanged = raisePropertyChanged ?? throw new ArgumentNullException(nameof(raisePropertyChanged));
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            _subject = new BehaviorSubject<T>(initialValue);
        }

        public string PropertyName { get; }

        public T Value
        {
            get
            {
                return _subject.Value;
            }
            set
            {
                if (!EqualityComparer<T>.Default.Equals(value, _subject.Value))
                {
                    _subject.OnNext(value);
                    _raisePropertyChanged(PropertyName);
                }
            }
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return _subject.Subscribe(observer);
        }

        public void Dispose()
        {
            _subject.Dispose();
        }

        void IObserver<T>.OnNext(T value)
        {
            Value = value;
        }

        void IObserver<T>.OnError(Exception error)
        {
            // Do nothing
        }

        void IObserver<T>.OnCompleted()
        {
            // Do nothing
        }
    }
}
