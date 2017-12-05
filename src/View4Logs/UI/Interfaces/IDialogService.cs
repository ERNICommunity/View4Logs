using System;

namespace View4Logs.UI.Interfaces
{
    public interface IDialogService
    {
        void RegisterHost(IDialogHost host);

        IObservable<TResult> ShowDialog<TResult>(IDialog<TResult> dialog);
    }
}
