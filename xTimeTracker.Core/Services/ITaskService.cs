namespace xTimeTracker.Core.Services
{
    public interface ITaskService
    {
        Task<bool> Create(Task task);
        Task<IEnumerable<Task>> GetTasksByProject(int projectId);
        Task<bool> Update(Task task);
        Task<bool> Delete(int taskId);
    }
}
