using System;
using System.IO;
using System.Threading;

namespace View4Logs.Utils.Streams
{
    /// <summary>
    /// Stream decorator which waits for available data on underlying stream until provided <see cref="CancellationToken"/> is canceled.
    /// </summary>
    public class BlockingRetryStream : Stream
    {
        private readonly int _retryDelay;
        private readonly CancellationToken _token;
        private bool _atEof;

        public BlockingRetryStream(Stream baseStream, int retryDelay, CancellationToken token)
        {
            _retryDelay = retryDelay;
            _token = token;
            BaseStream = baseStream;
        }

        /// <summary>
        /// Raised every time when underlying stream has no more available data.
        /// </summary>
        public event EventHandler EndOfStreamReached;

        public Stream BaseStream { get; }

        public override bool CanRead => BaseStream.CanRead;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => BaseStream.Position;
            set => BaseStream.Position = value;
        }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_token.IsCancellationRequested)
            {
                return 0;
            }

            int read;
            while ((read = BaseStream.Read(buffer, offset, count)) == 0)
            {
                if (!_atEof)
                {
                    _atEof = true;
                    EndOfStreamReached?.Invoke(this, EventArgs.Empty);
                }

                Thread.Sleep(_retryDelay);

                if (_token.IsCancellationRequested)
                {
                    return 0;
                }
            }

            _atEof = false;
            return read;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                BaseStream.Dispose();
            }
        }
    }
}