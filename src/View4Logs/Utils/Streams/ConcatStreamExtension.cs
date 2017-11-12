using System.IO;

namespace View4Logs.Utils.Streams
{
    public static class ConcatStreamExtension
    {
        public static Stream Concat(this Stream first, Stream second)
        {
            return new ConcatStream(first, second);
        }
    }
}
