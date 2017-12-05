using System;
using System.Linq;
using System.Collections.ObjectModel;
using View4Logs.Common.Interfaces;
using View4Logs.UI.Interfaces;

namespace View4Logs.UI.ViewModel
{
    public sealed class DialogHostViewModel : Base.ViewModel, IDialogHost
    {
        private DialogContainer _activeDialog;

        public DialogHostViewModel(IDialogService dialogService)
        {
            Dialogs = new ObservableCollection<DialogContainer>();
            dialogService.RegisterHost(this);
        }

        public DialogContainer ActiveDialog
        {
            get => _activeDialog;
            private set => Set(ref _activeDialog, value);
        }

        public ObservableCollection<DialogContainer> Dialogs { get; }

        public void Add(IDialog dialog)
        {
            var item = new DialogContainer(dialog);
            ActiveDialog = item;
            Dialogs.Add(item);
        }

        public void Remove(IDialog dialog)
        {
            var item = Dialogs.First(x => x.Dialog == dialog);
            Dialogs.Remove(item);
            if (item == ActiveDialog)
            {
                ActiveDialog = Dialogs.LastOrDefault();
            }
        }
    }

    public sealed class DialogContainer
    {
        public DialogContainer(IDialog dialog)
        {
            Dialog = dialog;
        }

        public IDialog Dialog { get; }
    }
}
