namespace xTimeTracker.Core.Services
{
    interface ITaskService
    {
        Task<bool> CreateTask(Task task);
        Task<IEnumerable<Task>> GetTasksByProject(int projectId);
        Task<bool> UpdateTask(Task task);
        Task<bool> DeleteProject(int taskId);
    }
}
