using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;

namespace View4Logs.Services
{
    public sealed class FileLogSource : ILogSource
    {
        private readonly Func<Stream, IEnumerable<LogMessage>> _processStream;
        private readonly IConnectableObservable<IList<LogMessage>> _messages;

        public FileLogSource(string path, Func<Stream, IEnumerable<LogMessage>> processStream)
        {
            _processStream = processStream;

            Name = Path.GetFileNameWithoutExtension(path);
            FullPath = Path.GetFullPath(path);

            _messages = Observable.Defer(() => ProcessFile().ToObservable().Buffer(4096)).Publish();
        }

        public string Name { get; }

        public string FullPath { get; }

        public void Start()
        {
            _messages.Connect();
        }

        public IDisposable Subscribe(IObserver<IList<LogMessage>> observer)
        {
            return _messages.Subscribe(observer);
        }

        private IEnumerable<LogMessage> ProcessFile()
        {
            using (var stream = File.OpenRead(FullPath))
            {
                foreach (var message in _processStream(stream))
                {
                    yield return message;
                }
            }
        }
    }
}