using System.Windows;
using System.Windows.Markup;

namespace View4Logs.UI.Base
{
    [ContentProperty(nameof(Template))]
    public abstract class View
    {
        public DataTemplate Template { get; set; }
    }
}
