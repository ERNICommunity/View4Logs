using System.Windows;

namespace View4Logs.UI.Theme
{
    public static class Button
    {        
        public static ComponentResourceKey SearchBarButton { get; } = new ComponentResourceKey(typeof(Button), nameof(SearchBarButton));

        public static ComponentResourceKey SideBarButton { get; } = new ComponentResourceKey(typeof(Button), nameof(SideBarButton));
        public static ComponentResourceKey SideBarToggleButton { get; } = new ComponentResourceKey(typeof(Button), nameof(SideBarToggleButton));

        public static ComponentResourceKey DialogHeaderButton { get; } = new ComponentResourceKey(typeof(Button), nameof(DialogHeaderButton));

        public static ComponentResourceKey DialogContentButton { get; } = new ComponentResourceKey(typeof(Button), nameof(DialogContentButton));
    }
}
