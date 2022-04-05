using Project.Common.CustomQuery;
using Project.Data.Domain.Account;
using Project.Data.Domain.Logging;
using System;
using System.Collections.Generic;

namespace Project.Service.Logging
{
    public partial interface ILogger
    {
        void DeleteLog(Log log);

        IPagedList<Log> GetAllLogs(DateTime? from, DateTime? to,string message, LogLevel? logLevel, int pageIndex, int pageSize);

        Log GetLogById(int logId);

        IList<Log> GetLogByIds(long[] logIds);

        Log InsertLog(LogLevel logLevel, string shortMessage, string fullMessage = "", User user = null);
    }
}
