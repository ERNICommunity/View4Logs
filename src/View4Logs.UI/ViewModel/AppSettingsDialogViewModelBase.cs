using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using View4Logs.UI.Base;
using View4Logs.UI.Control;
using View4Logs.UI.Interfaces;

namespace View4Logs.UI.ViewModel
{
    public class AppSettingsDialogViewModelBase : DialogViewModelBase<Unit>
    {
        public AppSettingsDialogViewModelBase(IDialogService dialogService)
        {
            // TODO: This impl is just for testing of the dialog support

            CloseCommand = Command.Create((object o) => Close());

            NestedCommand = Command.Create(async (object o) =>
            {
                await dialogService.ShowDialog(new AppSettingsDialog() { Label = Label + "X" }).DefaultIfEmpty();
            });
        }

        public ICommand CloseCommand { get; }

        public ICommand NestedCommand { get; }

        public string Label { get; set; }
    }
}