using System.Reactive;
using View4Logs.UI.View;
using View4Logs.UI.ViewModel;

namespace View4Logs.UI.Control
{
    public sealed class AppSettingsDialog : Dialog<AppSettingsDialogView, AppSettingsDialogViewModel, Unit>
    {
        public string Label { get; set; }
    }
}
