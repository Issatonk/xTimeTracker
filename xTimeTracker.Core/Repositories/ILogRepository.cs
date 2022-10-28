namespace xTimeTracker.Core.Repositories
{
    interface ILogRepository
    {
        Task<bool> CreateLog(Log log);
        Task<IEnumerable<Log>> GetLogs();
        Task<IEnumerable<Log>> GetLogsByDateInterval(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Log>> GetLogsByTask(int taskId);
        Task<bool> UpdateLog(Log log);
        Task<bool> DeleteLog(int logId);
    }
}
