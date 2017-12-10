using System.Windows.Input;
using View4Logs.Common.Interfaces;
using View4Logs.UI.Base;

namespace View4Logs.UI.ViewModel
{
    public sealed class LogSourceViewModel : Base.ViewModel
    {
        public LogSourceViewModel(ILogSourceService logSourceService)
        {
            RemoveCommand = Command.Create((object o) => Source.Dispose());
        }


        private ILogSource _source;
        public ILogSource Source
        {
            get => _source;
            set => Set(ref _source, value);
        }

        public ICommand RemoveCommand { get; }
    }
}
