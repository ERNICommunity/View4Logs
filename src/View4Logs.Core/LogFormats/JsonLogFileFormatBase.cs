using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace View4Logs.Core.LogFormats
{
    public abstract class JsonLogFileFormatBase : LogFileFormatBase
    {
        protected override bool CheckFilename(string path)
        {
            var ext = Path.GetExtension(path).ToLower();
            if (string.IsNullOrEmpty(ext) || ext == ".json" || ext == ".log")
            {
                return true;
            }

            return false;
        }

        protected override bool CheckContent(FileStream stream)
        {
            using (var textReader = new StreamReader(stream, Encoding.Default, true, 1024, true))
            using (JsonReader jsonReader = new JsonTextReader(textReader) { SupportMultipleContent = true })
            {
                if (jsonReader.Read())
                {
                    var obj = JObject.Load(jsonReader);
                    if (!CheckObject(obj))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        protected abstract bool CheckObject(JObject obj);
    }
}