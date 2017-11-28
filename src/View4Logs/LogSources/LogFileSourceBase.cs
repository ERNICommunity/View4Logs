using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;
using View4Logs.Utils.IO;

namespace View4Logs.LogSources
{
    /// <summary>
    /// Base class for processing log files.
    /// </summary>
    /// <remarks>
    /// Derived classes must implement <see cref="ProcessStream(FileStream)"/> method to read log messages from stream.
    /// Opening file stream and watching for changes is implemented here.
    /// Log source is reset when the file is re-created or it detected shrink in size.
    /// Otherwise it's assumed that data were appended.
    /// File is open in least restrictive mode and closed after it's read to (current) end.
    /// </remarks>
    public abstract class LogFileSourceBase : ILogSource
    {
        private readonly Subject<IList<LogMessage>> _messages;
        private readonly Subject<ILogSource> _reset;
        private readonly FileWatcher _fileWatcher;
        private bool _started;
        private bool _disposed;
        private long _position;

        protected LogFileSourceBase(string path)
        {
            Name = Path.GetFileNameWithoutExtension(path);
            FullPath = Path.GetFullPath(path);

            _messages = new Subject<IList<LogMessage>>();
            _reset = new Subject<ILogSource>();
            _fileWatcher = new FileWatcher(FullPath, TimeSpan.FromMilliseconds(500));

            Messages = _messages.AsObservable();
            Reset = _reset.AsObservable();
        }

        public string Name { get; }

        public string FullPath { get; }

        public IObservable<IList<LogMessage>> Messages { get; }

        public IObservable<ILogSource> Reset { get; }

        public void Start()
        {
            ThrowIfDisposed();

            if (_started)
            {
                throw new InvalidOperationException("File log source can be started only once.");
            }

            _started = true;
            WatchFile();
            ReadBatch();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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

                // File is bigger, we assume one or more log messages has been appended to the file.
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

                IList<LogMessage> messages = null;

                try
                {
                    messages = ProcessStream(fileStream);
                }
                catch (Exception ex)
                {
                    Clenaup();
                    _messages.OnError(ex);
                    return;
                }

                _position = fileStream.Position;

                if (messages.Count > 0)
                {
                    _messages.OnNext(messages);
                }
            }
        }

        protected abstract IList<LogMessage> ProcessStream(FileStream stream);

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

        private void Clenaup()
        {
            _messages?.Dispose();
            _reset?.Dispose();
            _fileWatcher?.Dispose();
        }
    }
}