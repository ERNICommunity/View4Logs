﻿using View4Logs.Common.Interfaces;
using View4Logs.LogSources;

namespace View4Logs.Services
{
    public sealed class JsonLogFileImportService : ILogFileImporter
    {
        private readonly ILogSourceService _logSourceService;

        public JsonLogFileImportService(ILogSourceService logSourceService)
        {
            _logSourceService = logSourceService;
        }

        public void Import(string filename)
        {
            var source = new JsonLogFileSource(filename);
            _logSourceService.Clear();
            _logSourceService.AddSource(source);
            source.Start();
        }
    }
}
