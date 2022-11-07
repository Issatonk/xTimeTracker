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
                var sqlQuery = "INSERT INTO Project (Name, [Plan], TimeSpent) VALUES(@Name, @Plan, @TimeSpent)";
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
                var sqlQuery = "SELECT * FROM Project";
                result  = _mapper.Map<
                    IEnumerable<Entities.Project>, 
                    IEnumerable<Core.Project>>(
                    await db.QueryAsync<Entities.Project>(sqlQuery));
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