using System.Windows;

namespace View4Logs.UI.Theme.Icons
{
    public sealed class SettingsIcon : System.Windows.Controls.Control
    {
        static SettingsIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SettingsIcon), new FrameworkPropertyMetadata(typeof(SettingsIcon)));
        }
    }
}