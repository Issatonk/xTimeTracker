using xTimeTracker.Core;
using xTimeTracker.Core.Repositories;
using xTimeTracker.Core.Services;

namespace xTimeTracker.BusinessLogic
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;

        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<bool> Create(Log log)
        {
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }
            return await _logRepository.CreateLog(log);
        }

        public async Task<IEnumerable<Log>> GetAll()
        {
            return await _logRepository.GetLogs();
        }

        public async Task<IEnumerable<Log>> GetLogsByDateInterval(DateTime startDate, DateTime endDate)
        {
            if(startDate.Date > endDate.Date)
            {
                throw new ArgumentException();
            }
            return await _logRepository.GetLogsByDateInterval(startDate, endDate);
        }

        public async Task<IEnumerable<Log>> GetLogsByTask(int taskId)
        {
            if (taskId <= 0)
            {
                throw new ArgumentException();
            }
            return await _logRepository.GetLogsByTask(taskId);
        }
        public async Task<IEnumerable<LogWithTaskNameAndProjectName>> GetLogsWithTaskNameAndProjectName()
        {
            var logs = await _logRepository.GetLogsWithProject();

            var result = logs.Select(l => new LogWithTaskNameAndProjectName
            {
                Id = l.Id,
                Date = l.Date,
                TimeSpent = l.TimeSpent,
                TaskName = l.Task.Name,
                ProjectName = l.Task.Project.Name
            });
            return result;

        }
        public async Task<bool> Update(Log log)
        {
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }
            return await _logRepository.UpdateLog(log);
        }
        public async Task<bool> Delete(int logId)
        {
            if (logId <= 0)
            {
                throw new ArgumentException();
            }
            return await _logRepository.DeleteLog(logId);
        }


    }
}