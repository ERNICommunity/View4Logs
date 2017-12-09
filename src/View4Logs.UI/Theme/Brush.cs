using System.Windows;

namespace View4Logs.UI.Theme
{
    public static class Brush
    {        
        #region LogLevel

        public static ComponentResourceKey LogLevelTrace { get; } = new ComponentResourceKey(typeof(Brush), nameof(LogLevelTrace));
        public static ComponentResourceKey LogLevelDebug { get; } = new ComponentResourceKey(typeof(Brush), nameof(LogLevelDebug));
        public static ComponentResourceKey LogLevelInfo { get; } = new ComponentResourceKey(typeof(Brush), nameof(LogLevelInfo));
        public static ComponentResourceKey LogLevelWarn { get; } = new ComponentResourceKey(typeof(Brush), nameof(LogLevelWarn));
        public static ComponentResourceKey LogLevelError { get; } = new ComponentResourceKey(typeof(Brush), nameof(LogLevelError));
        public static ComponentResourceKey LogLevelFatal { get; } = new ComponentResourceKey(typeof(Brush), nameof(LogLevelFatal));

        #endregion

        #region SideBar

        public static ComponentResourceKey SideBarBackground { get; } = new ComponentResourceKey(typeof(Brush), nameof(SideBarBackground));
        public static ComponentResourceKey SideBarForeground { get; } = new ComponentResourceKey(typeof(Brush), nameof(SideBarForeground));
        public static ComponentResourceKey SideBarForegroundHover { get; } = new ComponentResourceKey(typeof(Brush), nameof(SideBarForegroundHover));

        #endregion

        #region SearchBar

        public static ComponentResourceKey SearchBarBackground { get; } = new ComponentResourceKey(typeof(Brush), nameof(SearchBarBackground));
        public static ComponentResourceKey SearchBarBorder { get; } = new ComponentResourceKey(typeof(Brush), nameof(SearchBarBorder));
        public static ComponentResourceKey SearchBarForeground { get; } = new ComponentResourceKey(typeof(Brush), nameof(SearchBarForeground));

        public static ComponentResourceKey SearchBarForegroundPressed { get; } = new ComponentResourceKey(typeof(Brush), nameof(SearchBarForegroundPressed));
        public static ComponentResourceKey SearchBarForegroundHover { get; } = new ComponentResourceKey(typeof(Brush), nameof(SearchBarForegroundHover));

        public static ComponentResourceKey SearchBarButtonBackground { get; } = new ComponentResourceKey(typeof(Brush), nameof(SearchBarButtonBackground));
        public static ComponentResourceKey SearchBarButtonPressedBackground { get; } = new ComponentResourceKey(typeof(Brush), nameof(SearchBarButtonPressedBackground));
        public static  ComponentResourceKey SearchBarButtonBackgroundHover { get; } = new ComponentResourceKey(typeof(Brush), nameof(SearchBarButtonBackgroundHover));

        #endregion

        #region Logs

        public static  ComponentResourceKey LogsBackground { get; } = new ComponentResourceKey(typeof(Brush), nameof(LogsBackground));
        public static ComponentResourceKey LogsBorder { get; } = new ComponentResourceKey(typeof(Brush), nameof(LogsBorder));

        #endregion

        #region Dialog

        public static ComponentResourceKey DialogOverlayBackground { get; } = new ComponentResourceKey(typeof(Brush), nameof(DialogOverlayBackground));

        public static ComponentResourceKey DialogBackground { get; } = new ComponentResourceKey(typeof(Brush), nameof(DialogBackground));
        public static ComponentResourceKey DialogBorder { get; } = new ComponentResourceKey(typeof(Brush), nameof(DialogBorder));
        public static ComponentResourceKey DialogForeground { get; } = new ComponentResourceKey(typeof(Brush), nameof(DialogForeground));

        public static ComponentResourceKey DialogHeaderBackground { get; } = new ComponentResourceKey(typeof(Brush), nameof(DialogHeaderBackground));
        public static ComponentResourceKey DialogHeaderForeground { get; } = new ComponentResourceKey(typeof(Brush), nameof(DialogHeaderForeground));
        public static ComponentResourceKey DialogHeaderForegroundHover { get; } = new ComponentResourceKey(typeof(Brush), nameof(DialogHeaderForegroundHover));

        public static ComponentResourceKey DialogButtonBackground { get; } = new ComponentResourceKey(typeof(Brush), nameof(DialogButtonBackground));

        #endregion

        #region ScrollBar

        public static ComponentResourceKey ScrollBarBackground { get; } = new ComponentResourceKey(typeof(Brush), nameof(ScrollBarBackground));
        public static ComponentResourceKey ScrollBar { get; } = new ComponentResourceKey(typeof(Brush), nameof(ScrollBar));
        public static ComponentResourceKey ScrollBarHover { get; } = new ComponentResourceKey(typeof(Brush), nameof(ScrollBarHover));
        public static ComponentResourceKey ScrollBarPressed { get; } = new ComponentResourceKey(typeof(Brush), nameof(ScrollBarPressed));
        public static ComponentResourceKey ScrollBarDisabled { get; } = new ComponentResourceKey(typeof(Brush), nameof(ScrollBarDisabled));

        #endregion

        #region List

        public static ComponentResourceKey ListItemSelectedBackground { get; } = new ComponentResourceKey(typeof(Brush), nameof(ListItemSelectedBackground));
        public static ComponentResourceKey ListItemSelectedBorder { get; } = new ComponentResourceKey(typeof(Brush), nameof(ListItemSelectedBorder));

        #endregion

    }
}
