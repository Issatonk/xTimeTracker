using xTimeTracker.Core;
using xTimeTracker.Core.Repositories;
using xTimeTracker.Core.Services;


namespace xTimeTracker.BusinessLogic
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<bool> CreateProject(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException();
            }
            return await _projectRepository.CreateProject(project);
        }
        public async Task<IEnumerable<Project>> GetProjects()
        {
            return await _projectRepository.GetProjects();
        }
        public async Task<bool> UpdateProject(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException();
            }
            return await _projectRepository.UpdateProject(project);
        }

        public async Task<bool> DeleteProject(int projectId)
        {
            if (projectId <= 0)
            {
                throw new ArgumentException();
            }

            return await _projectRepository.DeleteProject(projectId);
        }
    }
}