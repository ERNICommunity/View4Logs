using System;
using System.Globalization;
using System.Reactive.Linq;
using View4Logs.Common;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;
using View4Logs.UI.Base;
using ILogsViewService = View4Logs.UI.Interfaces.ILogsViewService;

namespace View4Logs.UI.ViewModel
{
    public sealed class SearchPanelViewModel : Base.ViewModel, IDisposable
    {
        private readonly ObservableProperty<LogLevel> _selectedLogLevel;
        private readonly ObservableProperty<string> _query;

        public SearchPanelViewModel(ILogFilterService logFilterService, ILogsViewService logsViewService)
        {
            LogsViewService = logsViewService;

            _selectedLogLevel = CreateProperty(nameof(SelectedLogLevel), LogLevel.All);
            var levelFilter = _selectedLogLevel.Select(CreateLogLevelFilter);
            logFilterService.AddFilter(levelFilter);

            _query = CreateProperty<string>(nameof(Query));
            var queryFilter = _query.Select(CreateQueryFilter).DistinctUntilChanged();
            logFilterService.AddFilter(queryFilter);
        }

        public string Query
        {
            get => _query.Value;
            set => _query.Value = value;
        }

        public ILogsViewService LogsViewService { get; }

        public LogLevel SelectedLogLevel
        {
            get => _selectedLogLevel.Value;
            set => _selectedLogLevel.Value = value;
        }

        private Func<LogEvent, bool> CreateLogLevelFilter(LogLevel level)
        {
            if (level == LogLevel.All)
            {
                return LogFilter.PassAll;
            }

            return logEvent => logEvent.Level >= level;
        }

        private Func<LogEvent, bool> CreateQueryFilter(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return LogFilter.PassAll;
            }

            var culture = CultureInfo.CurrentCulture;

            return logEvent => culture.CompareInfo.IndexOf(logEvent.Message, query, CompareOptions.IgnoreCase) >= 0;
        }
    }
}
