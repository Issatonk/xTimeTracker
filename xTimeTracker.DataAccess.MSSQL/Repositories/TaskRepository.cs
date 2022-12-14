using AutoMapper;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using xTimeTracker.Core;
using xTimeTracker.Core.Repositories;
using xTimeTracker.DataAccess.MSSQL.Entities;

namespace xTimeTracker.DataAccess.MSSQL.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        public TaskRepository(ConnectionString connectionString, IMapper mapper)
        {
            _connectionString = connectionString.Connection;
            _mapper = mapper;
        }
        public async Task<bool> CreateTask(Core.Task task)
        {
            int result;
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO Task (Name, [Plan], ProjectId) VALUES(@Name, @Plan, @ProjectId)";
                result = await db.ExecuteAsync(sqlQuery, _mapper.Map<Core.Task, Entities.Task>(task));
            }
            return result == 0 ? false : true;
        }

        public async Task<IEnumerable<Core.Task>> GetTasksByProject(int projectId)
        {
            IEnumerable<Core.Task> result;
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "SELECT Task.Id, Task.Name, Task.[Plan], Task.ProjectId, SUM(Log.TimeSpent) AS TimeSpent FROM Task LEFT JOIN Log ON Task.Id = Log.TaskId GROUP BY Task.Id, Task.Name, Task.[Plan], Task.ProjectId HAVING Task.ProjectId = @projectId";
                var tasks =  await db.QueryAsync<Entities.Task>(sqlQuery, new {projectId});

                result = _mapper.Map<IEnumerable<Entities.Task>, IEnumerable<Core.Task>>(tasks);
            }
            return result;
        }

        public async Task<bool> UpdateTask(Core.Task task)
        {
            int result;
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "UPDATE Task SET Name = @Name, [Plan] = @Plan WHERE Id = @Id";
                result = await db.ExecuteAsync(sqlQuery, _mapper.Map<Core.Task, Entities.Task>(task));
            }
            return result == 0 ? false : true;
        }
        public async Task<bool> DeleteTask(int taskId)
        {
            int result;
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM Task WHERE Id = @taskId";
                result = await db.ExecuteAsync(sqlQuery, new { taskId });
            }
            return result == 0 ? false : true;
        }
    }
}