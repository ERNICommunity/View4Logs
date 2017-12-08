using System.Windows;

namespace View4Logs.UI.Theme
{
    public static class FontFamily
    {
        public static ComponentResourceKey Default { get; } = new ComponentResourceKey(typeof(FontFamily), nameof(Default));
        public static ComponentResourceKey FontAwesome { get; } = new ComponentResourceKey(typeof(FontFamily), nameof(FontAwesome));
        public static ComponentResourceKey MaterialIcons { get; } = new ComponentResourceKey(typeof(FontFamily), nameof(MaterialIcons));
    }
}
