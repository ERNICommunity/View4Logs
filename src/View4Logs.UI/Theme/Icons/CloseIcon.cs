using System.Windows;

namespace View4Logs.UI.Theme.Icons
{
    public sealed class CloseIcon : System.Windows.Controls.Control
    {
        static CloseIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CloseIcon), new FrameworkPropertyMetadata(typeof(CloseIcon)));
        }
    }
}