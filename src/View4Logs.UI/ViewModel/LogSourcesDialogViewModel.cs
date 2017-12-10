using System.Collections.Generic;
using System.Reactive;
using System.Windows.Input;
using View4Logs.Common.Interfaces;
using View4Logs.Common.Collections;
using View4Logs.UI.Base;

namespace View4Logs.UI.ViewModel
{
    public sealed class LogSourcesDialogViewModel : DialogViewModelBase<Unit>
    {
        private readonly ObservableProperty<IList<ILogSource>> _sources;

        public LogSourcesDialogViewModel(ILogSourceService logSourceService)
        {
            _sources = CreateProperty<IList<ILogSource>>(nameof(Sources));

            logSourceService.Sources.AsItemsBehaviorObservable().Subscribe(_sources);

            CloseCommand = Command.Create((object o) => Close());
        }

        public IList<ILogSource> Sources => _sources.Value;

        public ICommand CloseCommand { get; }
    }
}
