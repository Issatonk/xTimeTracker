namespace xTimeTracker.Core.Repositories
{
    public interface ILogRepository
    {
        Task<bool> CreateLog(Log log);
        Task<IEnumerable<Log>> GetLogs();
        Task<IEnumerable<Log>> GetLogsByDateInterval(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Log>> GetLogsByTask(int taskId);
        Task<IEnumerable<Log>> GetLogsWithProject();
        Task<bool> UpdateLog(Log log);
        Task<bool> DeleteLog(int logId);
    }
}
