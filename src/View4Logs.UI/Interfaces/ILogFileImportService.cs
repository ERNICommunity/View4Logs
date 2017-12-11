using System.Threading.Tasks;

namespace View4Logs.UI.Interfaces
{
    public interface ILogFileImportService
    {
        Task Import(string filename);
    }
}
