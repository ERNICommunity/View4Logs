using System;
using System.IO;

namespace View4Logs.Utils.Streams
{
    public sealed class ConcatTextReader : TextReader
    {
        private readonly TextReader[] _readers;
        private TextReader _current;
        private int _currentIndex;

        public ConcatTextReader(params TextReader[] readers)
        {
            _readers = readers ?? throw new ArgumentNullException(nameof(readers));

            if (_readers.Length > 0)
            {
                _current = _readers[0];
                _currentIndex = 0;
            }
        }

        public override int Peek()
        {
            while (_current != null)
            {
                var read = _current.Peek();
                if (read != -1)
                {
                    return read;
                }

                NextReader();
            }

            return -1;
        }


        public override int Read()
        {
            while (_current != null)
            {
                var read = _current.Read();
                if (read != -1)
                {
                    return read;
                }

                NextReader();
            }

            return -1;
        }

        public override int Read(char[] buffer, int index, int count)
        {
            while (_current != null)
            {
                var read = _current.Read(buffer, index, count);
                if (read > 0)
                {
                    return read;
                }

                NextReader();
            }

            return -1;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                for (var i = 0; i < _readers.Length; i++)
                {
                    _readers[i].Dispose();
                }
            }
        }

        private void NextReader()
        {
            _currentIndex++;
            _current = _currentIndex < _readers.Length ? _readers[_currentIndex] : null;
        }
    }
}