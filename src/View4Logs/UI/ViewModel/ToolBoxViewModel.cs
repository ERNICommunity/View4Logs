using System.Windows.Input;
using View4Logs.Base;
using View4Logs.UI.Control;
using View4Logs.UI.Interfaces;

namespace View4Logs.UI.ViewModel
{
    public sealed class ToolBoxViewModel : Base.ViewModel
    {
        public ToolBoxViewModel(IDialogService dialogService)
        {
            // TODO: This impl is just for testing of the dialog support
            OpenAppSettingsDialog = Command.Create<object>(o => dialogService.ShowDialog(new AppSettingsDialog() { Label = "X" }));
        }

        public ICommand OpenAppSettingsDialog { get; }
    }
}
