using System;
using System.IO;
using View4Logs.Common.Interfaces;

namespace View4Logs.Core.LogFormats
{
    public abstract class LogFileFormatBase : ILogFormat
    {
        public abstract string Name { get; }

        public bool CheckCompatibility(Uri uri)
        {
            try
            {
                if (!uri.IsFile)
                {
                    return false;
                }

                var path = uri.LocalPath;
                if (!CheckFilename(path))
                {
                    return false;
                }

                using (var fileStream = new FileStream(uri.LocalPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    if (!CheckContent(fileStream))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public abstract ILogSource CreateSource(Uri uri);

        protected virtual bool CheckFilename(string path) => true;

        protected abstract bool CheckContent(FileStream stream);
    }
}
