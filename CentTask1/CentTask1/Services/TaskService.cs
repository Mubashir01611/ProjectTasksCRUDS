using System.Threading.Tasks;
using CentTask1.DBC;
using CentTask1.DTO.TaskDtos;
using CentTask1.Interfaces;
using CentTask1.Models;
using CentTask1.ViewModels.ProjectViewModels;
using CentTask1.ViewModels.TaskViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace CentTask1.Services
{
    public class TaskService : ITaskService
    {
        private readonly DataContext _dataContext;

        public TaskService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        //create
        public async Task<TaskCreateViewModel> CreateTaskAsync(TaskCreateViewModel task)
        {
            var projectTask = new ProjectTask
            {
                TaskId = task.TaskId,
                TaskName = task.TaskName,
                Description = task.Description,
                StartDate = task.StartDate,
                EndDate = task.EndDate,
                Priority = task.Priority,
                AssignedTo = task.AssignedTo,
                TWR = task.TWR,
                EquipmentType = task.EquipmentType,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                ProjectId = int.TryParse(task.ProjectId, out var projectIdValue) ? projectIdValue : (int?)null
            };
            
                _dataContext.ProjectTasks.Add(projectTask);
                await _dataContext.SaveChangesAsync();
                return task;
            
        }


        //GetAllTasks
        public async Task<IEnumerable<TaskGetViewModel>> GetAllTasksAsync()
        {
            var tasks = _dataContext.ProjectTasks.Include(p => p.Project).Select(t => new TaskGetViewModel
            {
                TaskId = t.TaskId,
                TaskName = t.TaskName,
                Description = t.Description,
                StartDate = t.StartDate,
                EndDate = t.EndDate,
                EquipmentType = t.EquipmentType,
                AssignedTo = t.AssignedTo,
                Priority = t.Priority,
                TWR = t.TWR,
                ProjectName = t.Project != null ? t.Project.Name : null
            });
            return tasks;
        }

        //GetById
        public async Task<ProjectTask?> GetTaskByIdAsync(Guid id)
        {
            var projectTask = await _dataContext.ProjectTasks
                .FirstOrDefaultAsync(m => m.TaskId == id);
            return projectTask;
        }


        ////Update
        public async Task<TaskUpdateViewModel?> UpdateTaskAsync(Guid id, TaskUpdateViewModel updatedTask)
        {
            var existingTask = await _dataContext.ProjectTasks.FindAsync(id);
            if (existingTask == null)
            {
                // Handle not found: throw, return, or log as needed
                throw new InvalidOperationException($"Task with id {updatedTask.TaskId} not found.");
            }
            try
            {
                existingTask.TaskName = updatedTask.TaskName;
                existingTask.Description = updatedTask.Description;
                existingTask.StartDate = updatedTask.StartDate;
                existingTask.EndDate = updatedTask.EndDate;
                existingTask.EquipmentType = updatedTask.EquipmentType;
                existingTask.AssignedTo = updatedTask.AssignedTo;
                existingTask.Priority = updatedTask.Priority;
                existingTask.TWR = updatedTask.TWR;
                existingTask.ProjectId = int.TryParse(updatedTask.ProjectId, out var projectIdValue) ? projectIdValue : (int?)null;

                await _dataContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(updatedTask.TaskId))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            var taskViewModel = new TaskUpdateViewModel
            {
                TaskId = existingTask.TaskId,
                TaskName = existingTask.TaskName,
                Description = existingTask.Description,
                StartDate = existingTask.StartDate,
                EndDate = existingTask.EndDate,
                EquipmentType = existingTask.EquipmentType,
                AssignedTo = existingTask.AssignedTo,
                Priority = existingTask.Priority,
                TWR = existingTask.TWR,
                ProjectId = existingTask.ProjectId?.ToString()
                };
            return taskViewModel;
        }
        ////Delete
        public async Task<bool> DeleteTaskAsync(Guid id)
        {
            var task = await _dataContext.ProjectTasks
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (task == null)
            {
                return false;
            }
            _dataContext.ProjectTasks.Remove(task);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        private bool TaskExists(Guid id)
        {
            return _dataContext.ProjectTasks.Any(e => e.TaskId == id);
        }

    }
}
