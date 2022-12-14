using xTimeTracker.Core.Repositories;
using xTimeTracker.Core.Services;

namespace xTimeTracker.BusinessLogic
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        public async Task<bool> Create(Core.Task task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task) + " is null");
            }
            if(task.Id != 0 || string.IsNullOrWhiteSpace(task.Name) ||task.Plan.Ticks<=0)
            {
                throw new ArgumentException(nameof(task) + " is invalid");
            }
            return await _taskRepository.CreateTask(task);
        }
        public async Task<IEnumerable<Core.Task>> GetTasksByProject(int projectId)
        {
            if (projectId <= 0)
            {
                throw new ArgumentException();
            }
            return await _taskRepository.GetTasksByProject(projectId);
        }
        public async Task<bool> Update(Core.Task task)
        {
            if (task == null)
            {
                throw new ArgumentNullException();
            }
            if (task.Id <= 0 || string.IsNullOrWhiteSpace(task.Name) || task.Plan.Ticks <= 0)
            {
                throw new ArgumentException(nameof(task) + " is invalid");
            }
            return await _taskRepository.UpdateTask(task);
        }

        public async Task<bool> Delete(int taskId)
        {
            if (taskId <= 0)
            {
                throw new ArgumentException();
            }
            return await _taskRepository.DeleteTask(taskId);
        }
    }
}