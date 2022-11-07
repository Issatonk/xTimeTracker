using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xTimeTracker.Core.Services
{
    public interface IProjectService
    {
        Task<bool> CreateProject(Project project);
        Task<IEnumerable<Project>> GetProjects();
        Task<bool> UpdateProject(Project project);
        Task<bool> DeleteProject(int projectId);
    }
}
