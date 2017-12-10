using System.Windows;

namespace View4Logs.UI.Theme
{
    public static class Template
    {        
        public static ComponentResourceKey LogLevel { get; } = new ComponentResourceKey(typeof(Template), nameof(LogLevel));
        public static ComponentResourceKey LogLevelColorBox { get; } = new ComponentResourceKey(typeof(Template), nameof(LogLevelColorBox));
    }
}
