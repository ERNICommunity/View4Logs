using View4Logs.Theme;
using View4Logs.UI.Theme;

namespace View4Logs.UI.Interfaces
{
    public interface IThemeConfigurationService
    {
        ThemeResourceDictionary[] ColorThemes { get; }

        ThemeResourceDictionary SelectedColorTheme { get; set; }
    }
}
