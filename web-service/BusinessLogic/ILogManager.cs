using System.Collections.Generic;
using System.Threading.Tasks;

using Model;

namespace BusinessLogic
{
    public interface ILogManager
    {
        Task<List<LogEntry>> GetAll(string text);
    }
}
