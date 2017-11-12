using System.Threading.Tasks;

namespace View4Logs.Common.Interfaces
{
    public interface ILogFileImporter
    {
        Task Import(string filename);
    }
}
