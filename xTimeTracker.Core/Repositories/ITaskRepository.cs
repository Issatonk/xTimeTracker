namespace xTimeTracker.Core.Repositories
{
    interface ITaskRepository
    {
        Task<bool> CreateTask(Task task);
        Task<IEnumerable<Task>> GetTasksByProject(int projectId);
        Task<bool> UpdateTask(Task task);
        Task<bool> DeleteProject(int taskId);
    }
}
