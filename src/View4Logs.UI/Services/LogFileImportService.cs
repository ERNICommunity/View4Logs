using System;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using View4Logs.Common.Interfaces;
using View4Logs.UI.Control;
using View4Logs.UI.Interfaces;
using View4Logs.UI.ViewModel;

namespace View4Logs.UI.Services
{
    public sealed class LogFileImportService : DispatcherObject, ILogFileImportService
    {
        private readonly ILogSourceService _logSourceService;
        private readonly IList<ILogFormat> _logFormats;
        private readonly IDialogService _dialogService;

        public LogFileImportService(ILogSourceService logSourceService, IList<ILogFormat> logFormats, IDialogService dialogService)
        {
            _logSourceService = logSourceService;
            _logFormats = logFormats;
            _dialogService = dialogService;
        }

        public async Task Import(string filename)
        {
            CheckAccess();

            var uri = new Uri(filename);
            var availableFormats = _logFormats.Where(format => format.CheckCompatibility(uri)).ToList();

            if (availableFormats.Count == 0)
            {
                await _dialogService.ShowDialog(new MessageDialog("Log Import", "File format is not supported."));
            }
            else if (availableFormats.Count == 1)
            {
                await Import(uri, availableFormats[0]);
            }
            else
            {
                var format = await _dialogService.ShowDialog(new LogFormatSelectionDialog(uri, availableFormats));
                if (format != null)
                {
                    await Import(uri, format);
                }
            }
        }

        private async Task Import(Uri uri, ILogFormat format)
        {
            await Task.Run(() =>
            {
                var source = format.CreateSource(uri);
                _logSourceService.AddSource(source);
                source.Start();
            });
        }
    }
}