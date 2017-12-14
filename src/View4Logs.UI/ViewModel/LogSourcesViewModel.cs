using System.Collections.Generic;
using View4Logs.Common.Interfaces;
using View4Logs.Common.Collections;
using View4Logs.UI.Base;

namespace View4Logs.UI.ViewModel
{
    public sealed class LogSourcesViewModel : Base.ViewModel
    {
        private readonly ObservableProperty<IList<ILogSource>> _sources;

        public LogSourcesViewModel(ILogSourceService logSourceService)
        {
            _sources = CreateProperty<IList<ILogSource>>(nameof(Sources), logSourceService.Sources.AsItemsBehaviorObservable());
        }

        public IList<ILogSource> Sources => _sources.Value;
    }
}
