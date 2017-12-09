using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using View4Logs.UI.Base;
using View4Logs.UI.Interfaces;
using View4Logs.UI.ViewModel;

namespace View4Logs.UI.Control
{
    public abstract class Dialog<TView, TViewModel, TResult> : Component<TView, TViewModel>, IDialog<TResult>, IDisposable
        where TView : Base.View, IComponentConnector, new()
        where TViewModel : DialogViewModelBase<TResult>
    {
        private readonly ReplaySubject<TResult> _result;
        private IInputElement _previouslyFocusedElement;

        protected Dialog()
        {
            _result = new ReplaySubject<TResult>();
            Result = _result.AsObservable();
        }

        IObservable<object> IDialog.Result => Result.Select(x => (object)x);

        public IObservable<TResult> Result { get; }

        public IDisposable Subscribe(IObserver<TResult> observer)
        {
            return _result.Subscribe(observer);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            
            ViewModel.Result.Subscribe(_result);

            _previouslyFocusedElement = Keyboard.FocusedElement;
            Keyboard.ClearFocus();
            View?.Focus();
        }

        protected override void OnUnloaded()
        {
            if (_previouslyFocusedElement != null)
            {
                _previouslyFocusedElement.Focus();
                _previouslyFocusedElement = null;
            }

            base.OnUnloaded();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _result.Dispose();
            }
        }
    }
}
