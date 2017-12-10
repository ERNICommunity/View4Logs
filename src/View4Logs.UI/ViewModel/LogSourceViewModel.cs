using System.Windows.Input;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;
using View4Logs.UI.Base;

namespace View4Logs.UI.ViewModel
{
    public sealed class LogSourceViewModel : Base.ViewModel
    {
        private readonly ILogSourceLevelFilter _logSourceLevelFilter;

        public LogSourceViewModel(ILogSourceLevelFilter logSourceLevelFilter)
        {
            _logSourceLevelFilter = logSourceLevelFilter;
            RemoveCommand = Command.Create((object o) => Source.Dispose());
        }


        private ILogSource _source;
        public ILogSource Source
        {
            get => _source;
            set => Set(ref _source, value);
        }

        public LogLevel LogLevel
        {
            get => _logSourceLevelFilter.GetLogLevelForSource(Source);
            set => _logSourceLevelFilter.SetLogLevelForSource(Source, value);
        }

        public ICommand RemoveCommand { get; }
    }
}
