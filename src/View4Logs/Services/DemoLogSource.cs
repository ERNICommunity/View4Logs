using System;
using System.Reactive.Linq;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;

namespace View4Logs.Services
{
    public sealed class DemoLogSource
    {
        private readonly ILogSourceService _logSourceService;

        public DemoLogSource(ILogSourceService logSourceService)
        {
            _logSourceService = logSourceService;
        }

        public void Start()
        {
            var rand = new Random();
            var messages = new[]
            {
                "Some information",
                "Catastrophic error",
                "Just a warning, ignore me",
                "TRACE: nothing important"
            };

            Observable
                .Interval(TimeSpan.FromSeconds(2))
                .Select(_ => new LogMessage { Message = messages[rand.Next(0, messages.Length - 1)] })
                .Subscribe(_logSourceService.Append);

        }
    }
}
