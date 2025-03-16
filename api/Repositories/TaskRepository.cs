using api.Data;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDBContext _context;

    public TaskRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<List<MyTask>> GetAllAsync()
    {
        return await _context.Tasks
            .Where(t => t.DeletedAt == null)
            .ToListAsync();
    }

    public async Task<MyTask?> GetByIdAsync(int id)
    {
        return await _context.Tasks
            .Where(t => t.DeletedAt == null)
            .Include(t => t.TaskList)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<MyTask> Create(MyTask task)
    {
        if (task == null)
            throw new ArgumentNullException(nameof(task), "Task cannot be null");

        if (task.Id != 0)
            throw new InvalidOperationException("Trying to create task with predefined id");

        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<MyTask> Update(MyTask task, bool save = true)
    {
        var entry = _context.Entry(task);
        if (entry.Properties.Any(p => p.IsModified))
        {
            task.UpdatedAt = DateTime.UtcNow;

            if(save)
                await _context.SaveChangesAsync();
        }

        return task;
    }

    public async Task<MyTask> SoftDelete(MyTask task, bool save = true)
    {
        task.DeletedAt = DateTime.UtcNow;

        if(save)
            await _context.SaveChangesAsync();

        return task;
    }
}
