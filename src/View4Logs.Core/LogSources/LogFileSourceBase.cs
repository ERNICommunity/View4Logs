using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;
using View4Logs.Core.Utils;

namespace View4Logs.Core.LogSources
{
    /// <summary>
    /// Base class for processing log files.
    /// </summary>
    /// <remarks>
    /// Derived classes must implement <see cref="ProcessStream(FileStream)"/> method to read log events from stream.
    /// Opening file stream and watching for changes is implemented here.
    /// Log source is reset when the file is re-created or it detected shrink in size.
    /// Otherwise it's assumed that data were appended.
    /// File is open in least restrictive mode and closed after it's read to (current) end.
    /// </remarks>
    public abstract class LogFileSourceBase : ILogSource
    {
        private readonly Subject<IList<LogEvent>> _logEvents;
        private readonly Subject<ILogSource> _reset;
        private readonly FileWatcher _fileWatcher;
        private bool _started;
        private bool _disposed;
        private long _position;

        protected LogFileSourceBase(string path)
        {
            Name = Path.GetFileNameWithoutExtension(path);
            FullPath = Path.GetFullPath(path);

            _logEvents = new Subject<IList<LogEvent>>();
            _reset = new Subject<ILogSource>();
            _fileWatcher = new FileWatcher(FullPath, TimeSpan.FromMilliseconds(500));

            LogEvents = _logEvents.AsObservable();
            Reset = _reset.AsObservable();
        }

        public string Name { get; }

        public string FullPath { get; }

        public IObservable<IList<LogEvent>> LogEvents { get; }

        public IObservable<ILogSource> Reset { get; }

        public void Start()
        {
            ThrowIfDisposed();

            if (_started)
            {
                throw new InvalidOperationException("File log source can be started only once.");
            }

            Initialize();

            _started = true;
            WatchFile();
            ReadBatch();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Initialize()
        {
        }

        protected abstract IList<LogEvent> ProcessStream(FileStream stream);

        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Clenaup();
                _disposed = true;
            }
        }

        private void WatchFile()
        {
            // File was re-created
            _fileWatcher.CreationTime.Subscribe(_ =>
            {
                _position = 0;
                _reset.OnNext(this);
                ReadBatch();
            });

            // LastWriteTime is changed only after writing file stream is closed.
            // Therefore we have to watch file size to be able to detect new data if writer process
            // keeps file open (common performance optimization).
            _fileWatcher.Length.Subscribe(_ =>
            {
                // When file was re-created and already read by previous handler
                if (_fileWatcher.FileInfo.Length == _position)
                {
                    return;
                }

                // File is bigger, we assume one or more log events has been appended to the file.
                // if the file was modified in any other way, it will not work correctly.
                if (_fileWatcher.FileInfo.Length < _position)
                {
                    _position = 0;
                    _reset.OnNext(this);
                }

                ReadBatch();
            });
        }

        private void ReadBatch()
        {
            using (var fileStream = new FileStream(FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                if (fileStream.Length == _position)
                {
                    return;
                }

                fileStream.Position = _position;

                IList<LogEvent> logEvents = null;

                try
                {
                    logEvents = ProcessStream(fileStream);
                }
                catch (Exception ex)
                {
                    _logEvents.OnError(ex);
                    Clenaup();
                    return;
                }

                _position = fileStream.Position;

                if (logEvents.Count > 0)
                {
                    _logEvents.OnNext(logEvents);
                }
            }
        }

        private void Clenaup()
        {
            _logEvents?.Dispose();
            _reset?.Dispose();
            _fileWatcher?.Dispose();
        }
    }
}
