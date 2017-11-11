﻿using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Input;
using View4Logs.Base;

namespace View4Logs.Utils.Observables
{
    public sealed class ObservableCommand<TParam, TResult> : ViewModel, IObservable<TResult>, ICommand, IDisposable
    {
        private readonly Func<TParam, Task<TResult>> _execute;
        private Func<TParam, bool> _canExecuteCondition;
        private readonly IDisposable _canExecuteSubscription;

        private readonly ObservableProperty<bool> _isExecuting;
        private readonly Subject<TResult> _executeResults;

        public ObservableCommand(
            IObservable<Func<TParam, bool>> canExecute,
            Func<TParam, Task<TResult>> execute
        )
        {
            _execute = execute;
            _canExecuteSubscription = canExecute.Subscribe(OnCanExecuteNewValue);
            _isExecuting = CreateProperty<bool>(nameof(IsExecuting));
            _executeResults = new Subject<TResult>();
        }

        public event EventHandler CanExecuteChanged;

        public bool IsExecuting
        {
            get => _isExecuting.Value;
            private set => _isExecuting.Value = value;
        }

        public IObservable<bool> IsExecutingChanges => _isExecuting;

        public bool CanExecute(object parameter)
        {
            if (!(parameter is TParam param))
            {
                return false;
            }

            return CanExecute(param);
        }

        public bool CanExecute(TParam parameter)
        {
            return !IsExecuting && (_canExecuteCondition?.Invoke(parameter) ?? true);
        }

        public void Execute(object parameter)
        {
            // Coerce parameter to default value (important when TParam is value type).
            if (parameter == null)
            {
                parameter = default(TParam);
            }

            if (!(parameter is TParam param))
            {
                throw new ArgumentException($"Command requires parameters of type {typeof(TParam).Name}, but received parameter of type {parameter.GetType().Name}.", nameof(parameter));
            }

            Execute(param);
        }

        public void Execute(TParam parameter)
        {
            if (!CanExecute(parameter))
            {
                throw new InvalidOperationException("Command cannot be currently executed with provided parameter");
            }

            IsExecuting = true;
            _execute(parameter).ContinueWith(t =>
            {
                IsExecuting = false;
                if (t.IsCompleted)
                {
                    _executeResults.OnNext(t.Result);
                }
            });

        }

        public IDisposable Subscribe(IObserver<TResult> observer)
        {
            return _executeResults.Subscribe(observer);
        }

        public void Dispose()
        {
            _isExecuting.Dispose();
            _canExecuteSubscription.Dispose();
            _executeResults.Dispose();
        }

        private void OnCanExecuteNewValue(Func<TParam, bool> condition)
        {
            _canExecuteCondition = condition;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
