using System.Collections.Generic;
using System.Threading.Tasks;

using Model;

namespace DataAccess
{
    public interface ILogRepository
    {
        Task<List<LogEntry>> GetAll(string text);
    }
}
