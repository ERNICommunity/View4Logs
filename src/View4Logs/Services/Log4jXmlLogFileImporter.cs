using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;
using View4Logs.Utils.Xnl;

namespace View4Logs.Services
{
    public sealed class Log4jXmlLogFileImporter : ILogFileImporter
    {
        private const string Log4jNS = "http://logging.apache.org/log4j/2.0/events";
        private const string NLogNS = "http://www.nlog-project.org/schemas/NLog.xsd";

        private readonly ILogSourceService _logSourceService;

        public Log4jXmlLogFileImporter(ILogSourceService logSourceService)
        {
            _logSourceService = logSourceService;
        }

        public Task Import(string filename)
        {
            return Task.Run(() =>
            {
                var buf = new List<LogMessage>();

                using (var stream = File.OpenRead(filename))
                {
                    var xmlFragmentReader = new XmlFragmentReader(stream);
                    var ns = xmlFragmentReader.NamespaceManager;
                    ns.AddNamespace("log4j", Log4jNS);
                    ns.AddNamespace("nlog", NLogNS);

                    foreach (var el in xmlFragmentReader.Read())
                    {
                        var logMessage = new LogMessage
                        {
                            Message = el.Element(XName.Get("message", Log4jNS)).Value
                        };

                        buf.Add(logMessage);
                    }
                }

                _logSourceService.Reset(buf);
            });
        }
    }
}
