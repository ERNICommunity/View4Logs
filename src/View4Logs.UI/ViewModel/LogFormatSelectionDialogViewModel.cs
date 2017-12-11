using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Windows.Input;
using View4Logs.Common.Interfaces;
using View4Logs.UI.Base;

namespace View4Logs.UI.ViewModel
{
    public sealed class LogFormatSelectionDialogViewModel : DialogViewModelBase<ILogFormat>
    {
        private readonly ObservableProperty<ILogFormat> _selectedProperty;

        public LogFormatSelectionDialogViewModel(Uri sourceUri, IList<ILogFormat> options)
        {
            SourceUri = sourceUri;
            Options = options;

            _selectedProperty = CreateProperty<ILogFormat>(nameof(Selected));

            CloseCommand = Command.Create((object o) => Return(null));

            SelectCommand = Command.Create(
                _selectedProperty.Select(format => format != null),
                () => Return(Selected)
            );
        }

        public Uri SourceUri { get; }

        public IList<ILogFormat> Options { get; }
        
        public ILogFormat Selected
        {
            get => _selectedProperty.Value;
            set => _selectedProperty.Value = value;
        }

        public ICommand CloseCommand { get; }

        public ICommand SelectCommand { get; }
    }
}
