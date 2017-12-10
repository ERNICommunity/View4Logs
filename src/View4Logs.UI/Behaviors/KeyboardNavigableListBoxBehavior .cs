using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

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
            var container = AssociatedObject.ItemContainerGenerator.ContainerFromItem(AssociatedObject.SelectedItem) as UIElement;
            container?.Focus();
        }
    }
}
