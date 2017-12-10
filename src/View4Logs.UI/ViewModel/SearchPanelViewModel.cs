using System;
using System.Reactive.Linq;
using System.Windows.Input;
using View4Logs.Common;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;
using View4Logs.UI.Base;
using ILogsViewService = View4Logs.UI.Interfaces.ILogsViewService;

namespace View4Logs.UI.ViewModel
{
    public sealed class SearchPanelViewModel : Base.ViewModel, IDisposable
    {

        private readonly ObservableProperty<string> _query;

        public SearchPanelViewModel(ILogFilterService logFilterService, ILogsViewService logsViewService)
        {
            LogsViewService = logsViewService;
            _query = CreateProperty<string>(nameof(Query));
            var filter = _query.Select(CreateFilter).DistinctUntilChanged();
            logFilterService.AddFilter(filter);
        }

        public string Query
        {
            get => _query.Value;
            set => _query.Value = value;
        }

        public ILogsViewService LogsViewService { get; }

        private Func<LogEvent, bool> CreateFilter(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return LogFilter.PassAll;
            }

            return logEvent => logEvent.Message.Contains(query);
        }

        public void Dispose()
        {
            _query.Dispose();
        }
    }
}
