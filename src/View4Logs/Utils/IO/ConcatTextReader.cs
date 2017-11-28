using System;
using System.IO;

namespace View4Logs.Utils.IO
{
    /// <summary>
    /// Wraps array of input text readers and acts as a single text read which concatenates their content.
    /// </summary>
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
            int totalRead = 0;
            while (_current != null && totalRead < count)
            {
                var read = _current.Read(buffer, index + totalRead, count - totalRead);
                if (read > 0)
                {
                    totalRead += read;
                }
                else
                {
                    NextReader();
                }
            }

            return totalRead;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var reader in _readers)
                {
                    reader.Dispose();
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