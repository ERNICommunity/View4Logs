using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace View4Logs.UI.Behaviors
{
    public class KeyboardNavigableListBoxBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.SelectionChanged += OnAssociatedObjectSelectionChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SelectionChanged -= OnAssociatedObjectSelectionChanged;
        }

        private void OnAssociatedObjectSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                AssociatedObject.ScrollIntoView(e.AddedItems[0]);
            }

            if (Keyboard.FocusedElement is Visual element && element.IsAncestorOf(AssociatedObject))
            {
                var container = AssociatedObject.ItemContainerGenerator.ContainerFromItem(AssociatedObject.SelectedItem) as UIElement;
                container?.Focus();
            }
        }
    }
}
