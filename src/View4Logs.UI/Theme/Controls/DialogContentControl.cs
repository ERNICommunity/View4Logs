using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace View4Logs.UI.Theme.Controls
{
    public class DialogContentControl : HeaderedContentControl
    {
        public static readonly DependencyProperty HeaderBrushProperty = DependencyProperty.Register(nameof(HeaderBrush), typeof(System.Windows.Media.Brush), typeof(DialogContentControl));

        public System.Windows.Media.Brush HeaderBrush
        {
            get => (System.Windows.Media.Brush)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public static readonly DependencyProperty CloseCommandProperty = DependencyProperty.Register(nameof(CloseCommand), typeof(ICommand), typeof(DialogContentControl));

        public ICommand CloseCommand
        {
            get => (ICommand)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }
    }
}