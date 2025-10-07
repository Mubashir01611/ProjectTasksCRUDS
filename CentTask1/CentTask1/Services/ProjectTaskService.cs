using System.Threading.Tasks;
using CentTask1.DBC;
using CentTask1.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace CentTask1.Services
{
    public class ProjectTaskService
    {
        private readonly DataContext _dataContext;

        public ProjectTaskService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        //create
        public async Task<ProjectTask> CreateTaskAsync(ProjectTask task)
        {
            
                _dataContext.ProjectTasks.Add(task);
                await _dataContext.SaveChangesAsync();
                return task;
            
        }


        //GetAllTasks
        public async Task<IEnumerable<ProjectTask>> GetAllTasksAsync()
        {
            return await _dataContext.ProjectTasks.ToListAsync();
        }

        //GetById
        public async Task<ProjectTask?> GetTaskByIdAsync(int id)
        {
            var projectTask= await _dataContext.ProjectTasks
                .FirstOrDefaultAsync(m => m.id == id);
            return projectTask;
        }


        //Update
        public async Task<ProjectTask?> UpdateTaskAsync(int id, ProjectTask updatedTask)
        {
            var existingTask = await _dataContext.ProjectTasks.FindAsync(id);
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
