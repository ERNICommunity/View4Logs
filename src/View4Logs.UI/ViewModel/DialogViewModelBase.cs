using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace View4Logs.UI.ViewModel
{
    public abstract class DialogViewModelBase<TResult> : Base.ViewModel
    {
        private readonly Subject<TResult> _result;

        protected DialogViewModelBase()
        {
            _result = new Subject<TResult>();
            Result = _result.AsObservable();
        }

        public IObservable<TResult> Result { get; }

        public void Close()
        {
            _result.OnCompleted();
        }

        protected void Yield(TResult value)
        {
            _result.OnNext(value);
        }

        protected void Return(TResult value)
        {
            _result.OnNext(value);
            Close();
        }

        protected void ReturnError(Exception ex)
        {
            _result.OnError(ex);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _result.Dispose();
            }
        }
    }
}
