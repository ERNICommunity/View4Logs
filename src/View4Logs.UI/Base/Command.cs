using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace View4Logs.UI.Base
{
    public static class Command
    {
        public static ObservableCommand<TParam, Unit> Create<TParam>(Action<TParam> execute)
        {
            return new ObservableCommand<TParam, Unit>(
                Observable.Empty<Func<TParam, bool>>(),
                param =>
                {
                    execute(param);
                    return Task.FromResult(Unit.Default);
                }
            );
        }

        public static ObservableCommand<TParam, TResult> Create<TParam, TResult>(Func<TParam, TResult> execute)
        {
            return new ObservableCommand<TParam, TResult>(
                Observable.Empty<Func<TParam, bool>>(),
                param => Task.FromResult(execute(param))
            );
        }

        public static ObservableCommand<TParam, Unit> Create<TParam>(Func<TParam, Task> execute)
        {
            return new ObservableCommand<TParam, Unit>(
                Observable.Empty<Func<TParam, bool>>(),
                async param =>
                {
                    await execute(param);
                    return Unit.Default;
                }
            );
        }

        public static ObservableCommand<TParam, TResult> Create<TParam, TResult>(Func<TParam, Task<TResult>> execute)
        {
            return new ObservableCommand<TParam, TResult>(
                Observable.Empty<Func<TParam, bool>>(),
                execute
            );
        }

        public static ObservableCommand<TParam, Unit> Create<TParam>(Func<TParam, bool> canExecute, Action<TParam> execute)
        {
            return new ObservableCommand<TParam, Unit>(
                Observable.Return(canExecute),
                param =>
                {
                    execute(param);
                    return Task.FromResult(Unit.Default);
                }
            );
        }

        public static ObservableCommand<TParam, TResult> Create<TParam, TResult>(Func<TParam, bool> canExecute, Func<TParam, TResult> execute)
        {
            return new ObservableCommand<TParam, TResult>(
                Observable.Return(canExecute),
                param => Task.FromResult(execute(param))
            );
        }

        public static ObservableCommand<TParam, Unit> Create<TParam>(Func<TParam, bool> canExecute, Func<TParam, Task> execute)
        {
            return new ObservableCommand<TParam, Unit>(
                Observable.Return(canExecute),
                async param =>
                {
                    await execute(param);
                    return Unit.Default;
                }
            );
        }

        public static ObservableCommand<TParam, TResult> Create<TParam, TResult>(Func<TParam, bool> canExecute, Func<TParam, Task<TResult>> execute)
        {
            return new ObservableCommand<TParam, TResult>(
                Observable.Return(canExecute),
                execute
            );
        }

        public static ObservableCommand<TParam, Unit> Create<TParam>(IObservable<Func<TParam, bool>> canExecute, Action<TParam> execute)
        {
            return new ObservableCommand<TParam, Unit>(
                canExecute,
                param =>
                {
                    execute(param);
                    return Task.FromResult(Unit.Default);
                }
            );
        }

        public static ObservableCommand<TParam, TResult> Create<TParam, TResult>(IObservable<Func<TParam, bool>> canExecute, Func<TParam, TResult> execute)
        {
            return new ObservableCommand<TParam, TResult>(
                canExecute,
                param => Task.FromResult(execute(param))
            );
        }

        public static ObservableCommand<TParam, Unit> Create<TParam>(IObservable<Func<TParam, bool>> canExecute, Func<TParam, Task> execute)
        {
            return new ObservableCommand<TParam, Unit>(
                canExecute,
                async param =>
                {
                    await execute(param);
                    return Unit.Default;
                }
            );
        }
    }
}
