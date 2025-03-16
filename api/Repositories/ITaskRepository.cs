using api.Models;

namespace api.Repositories;

public interface ITaskRepository
{
    public Task<List<MyTask>> GetAllAsync();

    public Task<MyTask?> GetByIdAsync(int id);

    public Task<MyTask> Create(MyTask task);

    public Task<MyTask> Update(MyTask task, bool save = true);

    public Task<MyTask> SoftDelete(MyTask task, bool save = true);
}