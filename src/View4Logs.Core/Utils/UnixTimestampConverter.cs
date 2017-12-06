using System;

namespace View4Logs.Core.Utils
{
    public static class UnixTimestampConverter
    {
        public static DateTime ConvertFromMilliseconds(long timestamp)
        {
            var datetime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return datetime.AddMilliseconds(timestamp);
        }
    }
}