using System.Reactive;
using System.Windows.Input;
using View4Logs.UI.Base;
using View4Logs.UI.Interfaces;
using View4Logs.UI.Theme;

namespace View4Logs.UI.ViewModel
{
    public class AppSettingsDialogViewModel : DialogViewModelBase<Unit>
    {
        private readonly IThemeConfigurationService _themeConfigurationService;

        public AppSettingsDialogViewModel(IThemeConfigurationService themeConfigurationService)
        {
            _themeConfigurationService = themeConfigurationService;

            CloseCommand = Command.Create((object o) => Close());
        }

        public ICommand CloseCommand { get; }

        public ThemeResourceDictionary[] ColorThemes => _themeConfigurationService.ColorThemes;

        public ThemeResourceDictionary SelectedColorTheme
        {
            get => _themeConfigurationService.SelectedColorTheme;
            set => _themeConfigurationService.SelectedColorTheme = value;
        }
    }
}