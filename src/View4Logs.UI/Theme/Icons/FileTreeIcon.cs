using System.Windows;

namespace View4Logs.UI.Theme.Icons
{
    public sealed class FileTreeIcon : System.Windows.Controls.Control
    {
        static FileTreeIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FileTreeIcon), new FrameworkPropertyMetadata(typeof(FileTreeIcon)));
        }
    }
}