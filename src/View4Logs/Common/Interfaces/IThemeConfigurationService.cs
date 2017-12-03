using View4Logs.Theme;

namespace View4Logs.Common.Interfaces
{
    public interface IThemeConfigurationService
    {
        ThemeResourceDictionary[] ColorThemes { get; }

        ThemeResourceDictionary SelectedColorTheme { get; set; }
    }
}
