using System;
using View4Logs.Common.Data;

namespace View4Logs.Common
{
    public static class LogFilter
    {
        public static readonly Func<LogMessage, bool> PassAll = _ => true;
    }
}
