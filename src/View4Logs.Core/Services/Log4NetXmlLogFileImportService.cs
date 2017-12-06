using View4Logs.Common.Interfaces;
using View4Logs.Core.LogSources;

namespace View4Logs.Core.Services
{
    public sealed class Log4NetXmlLogFileImportService : ILogFileImporter
    {
        private readonly ILogSourceService _logSourceService;

        public Log4NetXmlLogFileImportService(ILogSourceService logSourceService)
        {
            _logSourceService = logSourceService;
        }

        public void Import(string filename)
        {
            var source = new Log4NetXmlLogFileSource(filename);
            _logSourceService.Clear();
            _logSourceService.AddSource(source);
            source.Start();
        }
    }
}
