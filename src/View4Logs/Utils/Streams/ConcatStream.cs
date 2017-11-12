using System;
using System.IO;

namespace View4Logs.Utils.Streams
{
    public sealed class ConcatStream : Stream
    {
        private readonly Stream _first;
        private readonly Stream _second;
        private bool _firstStreamFinished;

        public ConcatStream(Stream first, Stream second)
        {
            _first = first ?? throw new ArgumentNullException(nameof(first));
            _second = second ?? throw new ArgumentNullException(nameof(second));
        }

        public override bool CanRead => _first.CanRead && _second.CanRead;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => _first.Length + _second.Length;

        public override long Position { get; set; }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new System.NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new System.NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!CanRead)
            {
                throw new NotSupportedException();
            }

            if (!_firstStreamFinished)
            {
                var read = _first.Read(buffer, offset, count);

                if (read == 0)
                {
                    _firstStreamFinished = true;
                }

                if (read < count)
                {
                    read += _second.Read(buffer, offset + read, count - read);
                }

                return read; 
            }

            return _second.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}