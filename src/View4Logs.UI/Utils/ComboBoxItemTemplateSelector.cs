using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace View4Logs.UI.Utils
{
    public sealed class ComboBoxItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DropDownTemplate
        {
            get;
            set;
        }
        public DataTemplate SelectedTemplate
        {
            get;
            set;
        }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var current = container;

            do
            {
                current = VisualTreeHelper.GetParent(current);

                if (current is ComboBoxItem)
                {
                    return DropDownTemplate;
                }

                if (current is ComboBox)
                {
                    return SelectedTemplate;
                }

            } while (current != null);

            // Should never happen
            return SelectedTemplate;
        }
    }
}
