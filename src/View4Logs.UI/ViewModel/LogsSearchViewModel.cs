using System;
using System.Reactive.Linq;
using View4Logs.Common;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;
using View4Logs.UI.Base;

namespace View4Logs.UI.ViewModel
{
    public sealed class LogsSearchViewModel : Base.ViewModel, IDisposable
    {
        private readonly ObservableProperty<string> _query;

        public LogsSearchViewModel(ILogFilterService logFilterService)
        {
            _query = CreateProperty<string>(nameof(Query));
            var filter = _query.Select(CreateFilter).DistinctUntilChanged();
            logFilterService.AddFilter(filter);
        }

        public string Query
        {
            get => _query.Value;
            set => _query.Value = value;
        }

        private Func<LogMessage, bool> CreateFilter(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return LogFilter.PassAll;
            }

            return msg => msg.Message.Contains(query);
        }

        public void Dispose()
        {
            _query.Dispose();
        }
    }
}
