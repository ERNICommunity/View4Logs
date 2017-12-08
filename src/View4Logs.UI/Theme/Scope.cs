using System.Windows;

namespace View4Logs.UI.Theme
{
    public static class Scope
    {
        public static readonly ComponentResourceKey AppBar = new ComponentResourceKey(typeof(Scope), nameof(AppBar));


        public static readonly DependencyProperty NameProperty = DependencyProperty.RegisterAttached("Name", typeof(ResourceKey), typeof(Scope), new PropertyMetadata(OnNameChanged));

        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static ResourceKey GetName(FrameworkElement element)
        {
            return (ResourceKey)element.GetValue(NameProperty);
        }

        public static void SetName(FrameworkElement element, ResourceKey value)
        {
            element.SetValue(NameProperty, value);
        }

        private static void OnNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = d as FrameworkElement;
            var key = e.NewValue;

            if (target.TryFindResource(key) is ResourceDictionary resource)
            {
                target.Resources.MergedDictionaries.Add(resource);
            }
        }
    }
}