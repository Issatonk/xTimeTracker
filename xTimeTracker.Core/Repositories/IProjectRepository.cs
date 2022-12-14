using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xTimeTracker.Core.Repositories
{
    public interface IProjectRepository
    {
        Task<bool> CreateProject(Project project);
        Task<IEnumerable<Project>> GetProjects();
        Task<bool> UpdateProject(Project project);
        Task<bool> DeleteProject(int projectId);
        Task<IEnumerable<Project>> GetProjectsWithLogs(DateTime start, DateTime stop);
    }
}
