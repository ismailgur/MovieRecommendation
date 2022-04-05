using Project.Common.CustomQuery;
using Project.Data.Domain.Account;
using Project.Data.Domain.Logging;
using Project.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Service.Logging
{
    public class Logger : ILogger
    {
        private readonly IRepository<Log> _logRepository;


        public Logger(IRepository<Log> logRepository)
        {
            this._logRepository = logRepository;
        }


        public void DeleteLog(Log log)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            this._logRepository.Delete(log);
        }


        public IPagedList<Log> GetAllLogs(DateTime? from, DateTime? to, string message, LogLevel? logLevel, int pageIndex, int pageSize)
        {
            var query = this._logRepository.GetAll();

            if (from.HasValue)
                query = query.Where(l => from.Value <= l.InsertDateTime);
            if (to.HasValue)
                query = query.Where(l => to.Value >= l.InsertDateTime);

            if (logLevel.HasValue)
            {
                int logLevelId = (int)logLevel.Value;
                query = query.Where(l => logLevelId == l.LogLevelId);
            }
            if (!String.IsNullOrEmpty(message))
                query = query.Where(l => l.ShortMessage.Contains(message) || l.FullMessage.Contains(message));
            query = query.OrderByDescending(l => l.InsertDateTime);

            var log = new PagedList<Log>(query, pageIndex, pageSize);
            return log;
        }


        public Log GetLogById(int logId)
        {
            if (logId == 0)
                return null;

            var log = this._logRepository.Get(logId);
            return log;
        }


        public IList<Log> GetLogByIds(long[] logIds)
        {
            if (logIds == null || logIds.Length == 0)
                return new List<Log>();

            var query = from l in this._logRepository.GetAll()
                        where logIds.Contains(l.Id)
                        select l;
            var logItems = query.ToList();

            var sortedLogItems = new List<Log>();
            foreach (int id in logIds)
            {
                var log = logItems.Find(x => x.Id == id);
                if (log != null)
                    sortedLogItems.Add(log);
            }
            return sortedLogItems;
        }


        public Log InsertLog(LogLevel logLevel, string shortMessage, string fullMessage = "", User user = null)
        {
            var log = new Log()
            {
                LogLevel = logLevel,
                ShortMessage = shortMessage,
                FullMessage = fullMessage,
                UserId = user != null ? user.Id : null,
                InsertDateTime = DateTime.Now
            };

            this._logRepository.Add(log);

            return log;
        }
    }
}
