using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Windows;
using View4Logs.Theme;
using View4Logs.UI.Interfaces;
using View4Logs.UI.Theme;

namespace View4Logs.UI.Services
{
    public sealed class ThemeConfigurationService : IThemeConfigurationService
    {
        private ThemeResourceDictionary _selectedColorTheme;
        private readonly Collection<ResourceDictionary> _resources;

        public ThemeConfigurationService(IList<ThemeResourceDictionary> themeResources)
        {
            _resources = Application.Current.Resources.MergedDictionaries;

            ColorThemes = themeResources.Where(rd => rd.Category == typeof(Brush)).ToArray();
        }

        public ThemeResourceDictionary[] ColorThemes { get; }

        public ThemeResourceDictionary SelectedColorTheme
        {
            get => _selectedColorTheme;
            set
            {
                if (value == _selectedColorTheme)
                {
                    return;
                }

                if (_selectedColorTheme != null)
                {
                    _resources.Remove(_selectedColorTheme);
                }

                _selectedColorTheme = value;
                _resources.Add(_selectedColorTheme);
            }
        }

        public void LoadConfiguration()
        {
            var defualtColorTheme = ColorThemes.Single(rd => rd.IsDefault);
            ThemeResourceDictionary configuredColorTheme = null;
            var colorThemeName = ConfigurationManager.AppSettings["ColorTheme"];
            if (!string.IsNullOrEmpty(colorThemeName))
            {
                configuredColorTheme = ColorThemes.FirstOrDefault(rd => rd.Name == colorThemeName);
            }

            SelectedColorTheme = configuredColorTheme ?? defualtColorTheme;
        }
    }
}
