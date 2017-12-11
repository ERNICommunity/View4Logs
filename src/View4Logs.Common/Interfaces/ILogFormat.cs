using System;

namespace View4Logs.Common.Interfaces
{
    public interface ILogFormat
    {
        string Name { get; }

        bool CheckCompatibility(Uri uri);

        ILogSource CreateSource(Uri uri);
    }
}