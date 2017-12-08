using System.Windows;

namespace View4Logs.UI.Theme.Icons
{
    public sealed class FileIcon : System.Windows.Controls.Control
    {
        static FileIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FileIcon), new FrameworkPropertyMetadata(typeof(FileIcon)));
        }
    }
}