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

        protected override IList<LogMessage> ProcessStream(FileStream stream)
        {
            var result = new List<LogMessage>();

            using (var textReader = new StreamReader(stream, Encoding.Default, true, 1024, true))
            using (JsonReader jsonReader = new JsonTextReader(textReader) { SupportMultipleContent = true })
            {
                while (jsonReader.Read())
                {
                    var obj = JObject.Load(jsonReader);
                    var msg = ConvertObjectToLogMessage(obj);
                    result.Add(msg);
                }
            }

            return result;
        }

        protected abstract LogMessage ConvertObjectToLogMessage(JObject obj);
    }
}
