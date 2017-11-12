using System.Windows.Media;
using ColorValue = System.Windows.Media.Color;

namespace View4Logs.Theme
{
    public static class Color
    {
        public static readonly ColorValue AppBarBackground = (ColorValue)ColorConverter.ConvertFromString("#212121");
        public static readonly ColorValue Background = (ColorValue)ColorConverter.ConvertFromString("#303030");
        public static readonly ColorValue Border = (ColorValue)ColorConverter.ConvertFromString("#424242");
        public static readonly ColorValue HintText = (ColorValue)ColorConverter.ConvertFromString("#828489");
        public static readonly ColorValue Text = (ColorValue)ColorConverter.ConvertFromString("#c0c0c2");
    }
}
