using System.Windows;

namespace View4Logs.UI.Theme
{
    public static class FontSize
    {
        public static ComponentResourceKey ExtraSmall { get; } = new ComponentResourceKey(typeof(FontSize), nameof(ExtraSmall));
        public static ComponentResourceKey Small { get; } = new ComponentResourceKey(typeof(FontSize), nameof(Small));
        public static ComponentResourceKey Normal { get; } = new ComponentResourceKey(typeof(FontSize), nameof(Normal));
        public static ComponentResourceKey Large { get; } = new ComponentResourceKey(typeof(FontSize), nameof(Large));
        public static ComponentResourceKey ExtraLarge { get; } = new ComponentResourceKey(typeof(FontSize), nameof(ExtraLarge));
    }
}
