using System.Threading.Tasks;
using CentTask1.DBC;
using CentTask1.DTO.TaskDtos;
using CentTask1.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace CentTask1.Services
{
    public class TaskService
    {
        private readonly DataContext _dataContext;

        public TaskService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        //create
        public async Task<GetTaskDto> CreateTaskAsync(GetTaskDto task)
        {
            var projectTask = new ProjectTask
            {
                id = task.id,
                name = task.name,
                description = task.description,
                StartDate = task.StartDate,
                DueDate = task.DueDate,
                priority = task.priority,
                AssignedTo = task.AssignedTo,
                TWR = task.TWR,
                EquipmentType = task.EquipmentType,
                ProjectId = int.TryParse(task.ProjectId, out var projectIdValue) ? projectIdValue : (int?)null
            };
            
                _dataContext.ProjectTasks.Add(projectTask);
                await _dataContext.SaveChangesAsync();
                return task;
            
        }


        //GetAllTasks
        public async Task<IEnumerable<GetTaskDto>> GetAllTasksAsync()
        {
            var tasks =  _dataContext.ProjectTasks.Include(p => p.Project).Select(t => new GetTaskDto
            {
                id = t.id,
                name = t.name,
                description = t.description,
                StartDate = t.StartDate,
                DueDate = t.DueDate,
                EquipmentType = t.EquipmentType,
                AssignedTo = t.AssignedTo,
                priority = t.priority,
                TWR = t.TWR,
                ProjectName = t.Project != null ? t.Project.Name : null
            });
            return tasks;
        }

        //GetById
        public async Task<ProjectTask?> GetTaskByIdAsync(int id)
        {
            var projectTask= await _dataContext.ProjectTasks
                .FirstOrDefaultAsync(m => m.id == id);
            return projectTask;
        }


        //Update
        public async Task<ProjectTask?> UpdateTaskAsync(int id, GetTaskDto updatedTask)
        {
            var existingTask = await _dataContext.ProjectTasks.FindAsync(id);
            if (existingTask == null)
            {
                // Handle not found: throw, return, or log as needed
                throw new InvalidOperationException($"Task with id {updatedTask.id} not found.");
            }
            try
            {
                existingTask.name = updatedTask.name;
                existingTask.description = updatedTask.description;
                existingTask.StartDate = updatedTask.StartDate;
                existingTask.DueDate = updatedTask.DueDate;
                existingTask.EquipmentType = updatedTask.EquipmentType;
                existingTask.AssignedTo = updatedTask.AssignedTo;
                existingTask.priority = updatedTask.priority;  
                existingTask.TWR = updatedTask.TWR;
                existingTask.ProjectId = int.TryParse(updatedTask.ProjectId, out var projectIdValue) ? projectIdValue : (int?)null;

                await _dataContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(updatedTask.id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
            return existingTask;
        }
        //Delete
        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _dataContext.ProjectTasks
                .FirstOrDefaultAsync(m => m.id == id);
            if (task == null)
            {
                return false;
            }
            _dataContext.ProjectTasks.Remove(task);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        private bool TaskExists(int id)
        {
            return _dataContext.ProjectTasks.Any(e => e.id == id);
        }

    }
}
