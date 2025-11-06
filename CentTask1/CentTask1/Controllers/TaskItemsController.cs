using Microsoft.AspNetCore.Mvc; 
using Microsoft.EntityFrameworkCore;
using CentTask1.DBC;
using CentTask1.Entities;

namespace CentTask1.Controllers
{
    public class TaskItemsController : Controller
    {
        private readonly DataContext _context;

        public TaskItemsController(DataContext context)
        {
            _context = context;
        }

        // GET: TaskItems
        public IActionResult LoadTaskItems()
        {
            return PartialView("_GetTaskItems");
        }
        [HttpPost]
        public async Task<IActionResult> GetTaskItemsDataTable()
        { 
            var taskItems = await _context.TaskItem.Where(t => !t.IsDeleted).ToListAsync();
            return Json(taskItems);
        }
        [HttpPost]
        public async Task<IActionResult> CreateMultiple([FromBody] List<TaskItem> tasks)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            foreach (var task in tasks)
            {
                task.CreatedOn = DateTime.UtcNow;
                task.UpdatedOn = DateTime.UtcNow;
                await _context.TaskItem.AddAsync(task);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Tasks created successfully" });
        }
        // GET: TaskItems/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }

        // GET: TaskItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TaskItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskName,Description,Status,StartDate,EndDate,Id,IsDeleted,CreatedOn,UpdatedOn")] TaskItem taskItem)
        {
            if (ModelState.IsValid)
            {
                taskItem.Id = Guid.NewGuid();
                _context.Add(taskItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taskItem);
        }

        // GET: TaskItems/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItem.FindAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }
            return View(taskItem);
        }

        // POST: TaskItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TaskName,Description,Status,StartDate,EndDate,Id,IsDeleted,CreatedOn,UpdatedOn")] TaskItem taskItem)
        {
            if (id != taskItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskItemExists(taskItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(taskItem);
        }

        // GET: TaskItems/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }

        // POST: TaskItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var taskItem = await _context.TaskItem.FindAsync(id);
            if (taskItem != null)
            {
                _context.TaskItem.Remove(taskItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskItemExists(Guid id)
        {
            return _context.TaskItem.Any(e => e.Id == id);
        }
    }
}
