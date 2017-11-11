using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace View4Logs.Utils.Behaviors
{
    public sealed class DropFileBehavior : Behavior<UIElement>
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(DropFileBehavior));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Drop += OnAssociatedObjectDrop;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Drop -= OnAssociatedObjectDrop;
            base.OnDetaching();
        }

        private void OnAssociatedObjectDrop(object sender, DragEventArgs e)
        {
            var command = Command;
            if (command == null)
            {
                return;
            }

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (command.CanExecute(files))
                {
                    command.Execute(files);
                }
            }
        }
    }
}
