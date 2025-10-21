using CentTask1.DBC;
using CentTask1.Interfaces;
using CentTask1.Models;
using CentTask1.ViewModels.TaskViewModels;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace CentTask1.Services
{
    public class TaskService : ITaskService
    {
        private readonly ILogger<ProjectTask> _logger;
        private readonly DataContext _dataContext;

        public TaskService(DataContext dataContext,ILogger<ProjectTask> logger)
        {
            _logger = logger;
            _dataContext = dataContext;
        }
        //create
        public async Task<TaskCreateViewModel> CreateTaskAsync(TaskCreateViewModel task)
        {

            try
            {
                var projectTask = new ProjectTask
                {
                    Id = Guid.NewGuid(),
                    TaskName = task.TaskName,
                    Description = task.Description,
                    StartDate = task.StartDate,
                    EndDate = task.EndDate,
                    Priority = task.Priority,
                    //AssignedTo = task.AssignedTo,
                    TWR = task.TWR,
                    EquipmentType = task.EquipmentType,
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = false,
                    ProjectId = int.TryParse(task.ProjectId, out var projectIdValue) ? projectIdValue : (int?)null
                };

                _dataContext.ProjectTasks.Add(projectTask);
                await _dataContext.SaveChangesAsync();
                return new TaskCreateViewModel
                {
                    Id = projectTask.Id,
                    TaskName = projectTask.TaskName,
                    Message = "Task created successfully!"
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating project");
                // Log the exception (you can use a logging framework here)
                throw new InvalidOperationException("An error occurred while creating the task.", ex);
            }   
           
            
        }


        //GetAllTasks
        public async Task<IEnumerable<TaskGetViewModel>> GetAllTasksAsync()
        {
            try
            {
                var projectTasks = _dataContext.ProjectTasks
                .Where(pt => pt.IsDeleted == false)
                .Include(pt => pt.Project)
                .Select(t => new TaskGetViewModel
                {
                    Id = t.Id,
                    TaskName = t.TaskName,
                    Description = t.Description,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    EquipmentType = t.EquipmentType,
                    //AssignedTo = t.AssignedTo,
                    Priority = t.Priority,
                    TWR = t.TWR,
                    ProjectName = t.Project != null ? t.Project.Name : null
                });

                return projectTasks;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving project tasks");
                throw new InvalidOperationException("An error occurred while retrieving tasks.", ex);
            }
        }

        //GetById
        public async Task<TaskDetailViewModel?> GetTaskByIdAsync(Guid id)
        {
            var ddd = _dataContext.ProjectTasks.ToList();
            var projectTask = await _dataContext.ProjectTasks.Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.Id == id && m.IsDeleted == false);
            var taskDetailViewModel = projectTask == null ? null : new TaskDetailViewModel
            {
                Id = projectTask.Id,
                TaskName = projectTask.TaskName,
                Description = projectTask.Description,
                StartDate = projectTask.StartDate,
                EndDate = projectTask.EndDate,
                EquipmentType = projectTask.EquipmentType,
                Priority = projectTask.Priority,
                TWR = projectTask.TWR,
                ProjectName = projectTask.Project != null ? projectTask.Project.Name : null,
                ProjectId = projectTask.ProjectId.ToString() ?? null,
                CreatedOn = projectTask.CreatedOn,

            };
                //Assigned
            return taskDetailViewModel;
        }


        ////Update
        public async Task<TaskUpdateViewModel?> UpdateTaskAsync(Guid id, TaskUpdateViewModel updatedTask)
        {
            var existingTask = await _dataContext.ProjectTasks.FindAsync(id);
            if (existingTask == null)
            {
                // Handle not found: throw, return, or log as needed
                throw new InvalidOperationException($"Task with id {updatedTask.Id} not found.");
            }
            try
            {
                existingTask.TaskName = updatedTask.TaskName;
                existingTask.Description = updatedTask.Description;
                existingTask.StartDate = updatedTask.StartDate;
                existingTask.EndDate = updatedTask.EndDate;
                existingTask.EquipmentType = updatedTask.EquipmentType;
                //existingTask.AssignedTo = updatedTask.AssignedTo;
                existingTask.Priority = updatedTask.Priority;
                existingTask.TWR = updatedTask.TWR;
                existingTask.ProjectId = int.TryParse(updatedTask.ProjectId, out var projectIdValue) ? projectIdValue : (int?)null;

                await _dataContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(updatedTask.Id))
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
                Id = existingTask.Id,
                TaskName = existingTask.TaskName,
                Description = existingTask.Description,
                StartDate = existingTask.StartDate,
                EndDate = existingTask.EndDate,
                EquipmentType = existingTask.EquipmentType,
                //AssignedTo = existingTask.AssignedTo,
                Priority = existingTask.Priority,
                TWR = existingTask.TWR,
                ProjectId = existingTask.ProjectId?.ToString()
                };
            return taskViewModel;
        }
        ////Delete
        public async Task<bool> DeleteTaskAsync(Guid id)
        {
            try
            {
                var task = await _dataContext.ProjectTasks
              .FirstOrDefaultAsync(m => m.Id == id);
                if (task == null)
                {
                    return false;
                }
                task.IsDeleted = true;
                _dataContext.ProjectTasks.Update(task);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting project task");
                throw new InvalidOperationException("An error occurred while deleting the task.", ex);
            }
          
        }

        private bool TaskExists(Guid id)
        {
            return _dataContext.ProjectTasks.Any(e => e.Id == id);
        }

    }
}
