using System;
using Newtonsoft.Json.Linq;
using View4Logs.Common.Interfaces;
using View4Logs.Core.LogSources;

namespace View4Logs.Core.LogFormats
{
    public sealed class JsonLogFileFormat : JsonLogFileFormatBase
    {
        public override string Name => "JSON";

        protected override bool CheckObject(JObject obj)
        {
            // TODO
            return true;
        }

        public override ILogSource CreateSource(Uri uri)
        {
            return new JsonLogFileSource(uri.LocalPath);
        }
    }
}
