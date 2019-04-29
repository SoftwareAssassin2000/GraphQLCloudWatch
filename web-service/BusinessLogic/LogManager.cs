using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Model;
using DataAccess;

namespace BusinessLogic
{
    public class LogManager : ILogManager
    {
        #region Construction & Dependencies

        private ILogRepository LogRepository { get; set; }

        public LogManager()
            : this(new CloudWatchLogRepository())
        { }
        public LogManager(ILogRepository logRepository)
        {
            this.LogRepository = logRepository;
        }

        #endregion

        public async Task<List<LogEntry>> GetAll(string text)
        {
            //read data from log
            return await this.LogRepository.GetAll(text);
        }
    }
}
