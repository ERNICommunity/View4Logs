using View4Logs.Common.Interfaces;
using View4Logs.LogSources;

namespace View4Logs.Services
{
    public sealed class Log4JXmlLogFileImportService : ILogFileImporter
    {
        private readonly ILogSourceService _logSourceService;

        public Log4JXmlLogFileImportService(ILogSourceService logSourceService)
        {
            _logSourceService = logSourceService;
        }

        public void Import(string filename)
        {
            var source = new Log4JXmlLogFileSource(filename);
            _logSourceService.Clear();
            _logSourceService.AddSource(source);
            source.Start();
        }
    }
}
