using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using View4Logs.Common.Data;

namespace View4Logs.Core.LogSources
{
    public abstract class JsonLogFileSourceBase : LogFileSourceBase
    {
        protected JsonLogFileSourceBase(string path)
            : base(path)
        {
        }

        protected override IList<LogEvent> ProcessStream(FileStream stream)
        {
            var result = new List<LogEvent>();

            using (var textReader = new StreamReader(stream, Encoding.Default, true, 1024, true))
            using (JsonReader jsonReader = new JsonTextReader(textReader) { SupportMultipleContent = true })
            {
                while (jsonReader.Read())
                {
                    var obj = JObject.Load(jsonReader);
                    var msg = ConvertObjectToLogEvent(obj);
                    result.Add(msg);
                }
            }

            return result;
        }

        protected abstract LogEvent ConvertObjectToLogEvent(JObject obj);
    }
}
