using System;
using System.Reactive.Linq;
using System.Windows.Threading;
using View4Logs.UI.Interfaces;

namespace View4Logs.UI.Services
{
    public class DialogService : DispatcherObject, IDialogService
    {
        private IDialogHost _host;

        public void RegisterHost(IDialogHost host)
        {
            VerifyAccess();

            if (_host != null)
            {
                throw new InvalidOperationException("Dialog host is already registered");
            }

            _host = host ?? throw new ArgumentNullException(nameof(host));
        }

        public IObservable<TResult> ShowDialog<TResult>(IDialog<TResult> dialog)
        {
            VerifyAccess();

            if (_host == null)
            {
                throw new InvalidOperationException("No dialog host is registered.");
            }

            _host.Add(dialog);

            dialog.Result.Finally(() =>
            {
                _host.Remove(dialog);
            })
            .Subscribe();

            return dialog.Result;
        }
    }
}
