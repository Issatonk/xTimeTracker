using AutoMapper;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using xTimeTracker.Core;
using xTimeTracker.Core.Repositories;
using xTimeTracker.DataAccess.MSSQL.Entities;

namespace xTimeTracker.DataAccess.MSSQL.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;

        public ProjectRepository(ConnectionString connectionString, IMapper mapper)
        {
            _connectionString = connectionString.Connection;
            _mapper = mapper;
        }
        public async Task<bool> CreateProject(Core.Project project)
        {
            int result;
            using(IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO Project (Name, [Plan]) VALUES(@Name, @Plan)";
                var tempProject = _mapper.Map<Core.Project, Entities.Project>(project);
                result = await db.ExecuteAsync(sqlQuery, tempProject);
            }
            return result == 0? false : true;
        }

        public async Task<IEnumerable<Core.Project>> GetProjects()
        {
            IEnumerable<Core.Project> result;
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "  SELECT Project.Id,Project.Name,Project.[Plan], ISNULL(SUM(Log.TimeSpent), 0) AS TimeSpent FROM Project LEFT JOIN Task ON Project.Id = Task.ProjectId LEFT JOIN Log ON Task.Id = LOG.TaskId GROUP BY Project.Id, Project.Name, Project.[Plan]";
                result  = _mapper.Map<
                    IEnumerable<Entities.Project>, 
                    IEnumerable<Core.Project>>(
                    await db.QueryAsync<Entities.Project>(sqlQuery));
            }
            return result;
        }

        public async Task<IEnumerable<Core.Project>> GetProjectsWithLogs(DateTime start, DateTime end)
        {
            IEnumerable<Core.Project> result;
            string projectsQuery = "SELECT * FROM Project";
            string tasksQuery = "SELECT * FROM Task WHERE Id IN (SELECT TaskId FROM Log WHERE Date BETWEEN @start AND @end)";
            string logsQuery = "SELECT * FROM Log WHERE Date BETWEEN @start AND @end";
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var projects = await db.QueryAsync<Entities.Project>(projectsQuery);
                var tasks = await db.QueryAsync<Entities.Task>(tasksQuery, new { start, end });
                var logs = await db.QueryAsync<Entities.Log>(logsQuery, new { start, end });

                foreach (var project in projects)
                {
                    project.Tasks = tasks.Where(t => t.ProjectId == project.Id).ToList();
                    foreach (var task in project.Tasks)
                    {
                        task.Logs = logs.Where(l => l.TaskId == task.Id).ToList();
                    }
                }

                result = _mapper.Map<IEnumerable<Entities.Project>, IEnumerable<Core.Project>>(projects);
            }
            return result;
        }

        public async Task<bool> UpdateProject(Core.Project project)
        {
            int result;
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "UPDATE Project SET Name = @Name, [Plan] = @Plan WHERE Id = @Id";
                result = await db.ExecuteAsync(sqlQuery, _mapper.Map<Core.Project, Entities.Project>(project));
            }
            return result == 0 ? false : true;
        }

        public async Task<bool> DeleteProject(int projectId)
        {
            int result;
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM Project WHERE Id = @projectId";
                result = await db.ExecuteAsync(sqlQuery, new { projectId });
            }
            return result == 0 ? false : true;
        }

    }
}