using System.Windows;
using System.Windows.Markup;

namespace View4Logs.Core.MVVMComponent
{
    [ContentProperty(nameof(Template))]
    public abstract class View
    {
        public DataTemplate Template { get; set; }
    }
}
