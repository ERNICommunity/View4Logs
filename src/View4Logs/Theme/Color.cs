using System.Windows;

namespace View4Logs.Theme
{
    public static class Color
    {
        public static readonly ComponentResourceKey Background = new ComponentResourceKey(typeof(Color), nameof(Background));
        public static readonly ComponentResourceKey Border = new ComponentResourceKey(typeof(Color), nameof(Border));
        public static readonly ComponentResourceKey HintText = new ComponentResourceKey(typeof(Color), nameof(HintText));
        public static readonly ComponentResourceKey Text = new ComponentResourceKey(typeof(Color), nameof(Text));
    }
}
