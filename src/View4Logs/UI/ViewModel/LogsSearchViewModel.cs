using System;
using System.Reactive.Linq;
using View4Logs.Common;
using View4Logs.Services;
using View4Logs.Utils;

namespace View4Logs.UI.ViewModel
{
    public sealed class LogsSearchViewModel : Base.ViewModel
    {
        private readonly ILogFilterService _logFilterService;

        public LogsSearchViewModel(ILogFilterService logFilterService)
        {
            _logFilterService = logFilterService;

            _query = CreateProperty<string>(nameof(Query));
        }

        private readonly ObservableProperty<string>  _query;
        public string Query
        {
            get => _query.Value;
            set => _query.Value = value;
        }
    }
}
