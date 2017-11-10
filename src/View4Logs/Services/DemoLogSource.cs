using System;
using System.Linq;
using System.Reactive.Linq;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;

namespace View4Logs.Services
{
    public sealed class DemoLogSource
    {
        private readonly ILogSourceService _logSourceService;

        private readonly Random _rand = new Random();

        private readonly string[] _messages =
        {
            "Some information",
            "Catastrophic error",
            "Just a warning, ignore me",
            "TRACE: nothing important"
        };

        public DemoLogSource(ILogSourceService logSourceService)
        {
            _logSourceService = logSourceService;
        }

        public void Start()
        {
            foreach (var _ in Enumerable.Range(1, 100))
            {
                _logSourceService.Append(GenerateMessage());
            }

            ////Observable
            ////    .Interval(TimeSpan.FromSeconds(2))
            ////    .Select(_ => GenerateMessage())
            ////    .Subscribe(_logSourceService.Append);
        }

        private LogMessage GenerateMessage()
        {
            return new LogMessage {Message = _messages[_rand.Next(0, _messages.Length - 1)]};
        }
    }
}
