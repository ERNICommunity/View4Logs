using System.Windows;

namespace View4Logs.UI.Theme
{
    public static class Brush
    {
        public static ComponentResourceKey AppBarBackground { get; } = new ComponentResourceKey(typeof(Brush), nameof(AppBarBackground));
        public static ComponentResourceKey Background { get; } = new ComponentResourceKey(typeof(Brush), nameof(Background));
        public static ComponentResourceKey Border { get; } = new ComponentResourceKey(typeof(Brush), nameof(Border));
        public static ComponentResourceKey HintText { get; } = new ComponentResourceKey(typeof(Brush), nameof(HintText));
        public static ComponentResourceKey SideBarBackground { get; } = new ComponentResourceKey(typeof(Brush), nameof(SideBarBackground));
        public static ComponentResourceKey Overlay { get; } = new ComponentResourceKey(typeof(Brush), nameof(Overlay));
        public static ComponentResourceKey Text { get; } = new ComponentResourceKey(typeof(Brush), nameof(Text));
    }
}
