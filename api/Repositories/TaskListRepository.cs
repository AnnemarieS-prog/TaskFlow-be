using api.Data;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class TaskListRepository : ITaskListRepository
{
    private readonly ApplicationDBContext _context;

    public TaskListRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<List<TaskList>> GetAllAsync()
    {
        return await _context.TaskLists
            .Where(l => l.DeletedAt == null)
            .Include(l => l.Tasks)
            .ToListAsync();
    }

    public async Task<TaskList?> GetByIdAsync(int id)
    {
        return await _context.TaskLists
            .Where(l => l.DeletedAt == null)
            .Include(l => l.Tasks)
            .FirstOrDefaultAsync(l => l.Id == id);
    }
    public async Task<TaskList> CreateAsync(TaskList taskList)
    {
        if (taskList == null)
            throw new ArgumentNullException(nameof(taskList), "Task cannot be null");

        if (taskList.Id != 0)
            throw new InvalidOperationException("Trying to create task with predefined id");

        await _context.TaskLists.AddAsync(taskList);
        await _context.SaveChangesAsync();
        return taskList;
    }

    public async Task<TaskList> UpdateAsync(TaskList taskList)
    {
        var entry = _context.Entry(taskList);
        if (entry.Properties.Any(p => p.IsModified))
        {
            taskList.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        return taskList;
    }
    public async Task<TaskList> SoftDelete(TaskList taskList)
    {
        taskList.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return taskList;
    }
}
