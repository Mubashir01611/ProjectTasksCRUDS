using CentTask1.Models;
using CentTask1.ViewModels.TaskViewModels;

namespace CentTask1.Interfaces
{
    public interface ITaskService
    {
        Task<TaskCreateViewModel> CreateTaskAsync(TaskCreateViewModel task);
        Task<IEnumerable<TaskGetViewModel>> GetAllTasksAsync();
        Task<TaskDetailViewModel?> GetTaskByIdAsync(Guid id);
        Task<TaskUpdateViewModel?> UpdateTaskAsync(Guid id, TaskUpdateViewModel updatedTask);
        Task<bool> DeleteTaskAsync(Guid id); 
    }
}
