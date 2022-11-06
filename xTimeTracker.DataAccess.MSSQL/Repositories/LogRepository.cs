using AutoMapper;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using xTimeTracker.Core;
using xTimeTracker.Core.Repositories;

namespace xTimeTracker.DataAccess.MSSQL.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        public LogRepository(ConnectionString connectionString, IMapper mapper)
        {
            _connectionString = connectionString.Connection;
            _mapper = mapper;
        }

        public async Task<bool> CreateLog(Log log)
        {
            int result;
            string date2 = DateTime.UtcNow.ToString("yyyy-mm-dd");
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO Log ([Date], [TimeSpent], [TaskId]) VALUES(@Date, @TimeSpent, @TaskId)";
                result = await db.ExecuteAsync(sqlQuery, _mapper.Map<Core.Log, Entities.Log>(log));
            }
            return result == 0 ? false : true;
        }

        public async Task<IEnumerable<Log>> GetLogs()
        {
            IEnumerable<Log> result;
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "SELECT * FROM Log ORDER BY Date";
                result = _mapper.Map<
                    IEnumerable<Entities.Log>, 
                    IEnumerable<Core.Log>>( 
                    await db.QueryAsync<Entities.Log>(sqlQuery));
            }
            return result;
        }

        public async Task<IEnumerable<Log>> GetLogsByDateInterval(DateTime startDate, DateTime endDate)
        {
            IEnumerable<Log> result;
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "SELECT * FROM Log WHERE Date BETWEEN @startDate AND @endDate ORDER BY Date";
                result = _mapper.Map<
                    IEnumerable<Entities.Log>,
                    IEnumerable<Core.Log>>(
                    await db.QueryAsync<Entities.Log>(sqlQuery, new { startDate, endDate }));
            }
            return result;
        }

        public async Task<IEnumerable<Log>> GetLogsByTask(int taskId)
        {
            IEnumerable<Log> result;
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "SELECT * FROM Log WHERE taskId = @taskId ORDER BY Date";
                result = _mapper.Map<
                    IEnumerable<Entities.Log>,
                    IEnumerable<Core.Log>>(
                    await db.QueryAsync<Entities.Log>(sqlQuery, new {taskId}));
            }
            return result;
        }

        public async Task<bool> UpdateLog(Log log)
        {
            int result;
            using(IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "UPDATE Log SET Date = @Date, TimeSpent = @TimeSpent WHERE Id = @Id";
                result = await db.ExecuteAsync(sqlQuery, _mapper.Map<Core.Log, Entities.Log>(log));
            }
            return result == 0 ? false : true;
        }
        public async Task<bool> DeleteLog(int logId)
        {
            int result;
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM Log WHERE Id = @logId";
                result = await db.ExecuteAsync(sqlQuery, new { logId });
            }
            return result == 0 ? false : true;
        }
    }
}