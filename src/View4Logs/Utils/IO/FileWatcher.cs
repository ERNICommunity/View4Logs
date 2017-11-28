using System;
using System.IO;
using System.Reactive.Linq;

namespace View4Logs.Utils.IO
{
    public sealed class FileWatcher : IDisposable
    {
        private readonly IDisposable _subscription;

        public FileWatcher(string fileName, TimeSpan interval)
        {
            FileInfo = new FileInfo(fileName);

            var fileInfoObservable = Observable
                .Interval(interval)
                .Do(_ => Refresh())
                .Publish();

            CreationTime = fileInfoObservable.Select(_ => FileInfo.CreationTime).DistinctUntilChanged().Skip(1);
            LastWriteTime = fileInfoObservable.Select(_ => FileInfo.LastWriteTime).DistinctUntilChanged().Skip(1);
            Length = fileInfoObservable.Select(_ => FileInfo.Length).DistinctUntilChanged().Skip(1);

            _subscription = fileInfoObservable.Connect();
        }

        public FileInfo FileInfo { get; }

        public IObservable<DateTime> CreationTime { get; }
        public IObservable<DateTime> LastWriteTime { get; }
        public IObservable<long> Length { get; }

        public void Dispose()
        {
            _subscription.Dispose();
        }

        private void Refresh()
        {
            FileInfo.Refresh();
            if (FileInfo.Exists)
            {
                using (var fs = FileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    fs.ReadByte();
                }

                FileInfo.Refresh();
            }
        }
    }
}