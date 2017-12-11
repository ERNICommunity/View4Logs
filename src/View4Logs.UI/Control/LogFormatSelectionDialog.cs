using System;
using System.Collections.Generic;
using View4Logs.Common.Interfaces;
using View4Logs.UI.View;
using View4Logs.UI.ViewModel;

namespace View4Logs.UI.Control
{
    public sealed class LogFormatSelectionDialog : Dialog<LogFormatSelectionDialogView, LogFormatSelectionDialogViewModel, ILogFormat>
    {
        public LogFormatSelectionDialog(Uri sourceUri, IList<ILogFormat> options)
        {
            SourceUri = sourceUri;
            Options = options;
        }

        public Uri SourceUri { get; }
        public IList<ILogFormat> Options { get; }

        protected override LogFormatSelectionDialogViewModel ViewModelFactory()
        {
            return new LogFormatSelectionDialogViewModel(SourceUri, Options);
        }
    }
}
