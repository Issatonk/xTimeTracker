namespace xTimeTracker.Core.Services
{
    public interface ILogService
    {
        Task<bool> Create(Log log);
        Task<IEnumerable<Log>> GetAll();
        Task<IEnumerable<Log>> GetLogsByDateInterval(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Log>> GetLogsByTask(int taskId);
        Task<bool> Update(Log log);
        Task<bool> Delete(int logId);
    }
}
