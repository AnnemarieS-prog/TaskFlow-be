using api.Models;

namespace api.Repositories;

public interface ITaskListRepository
{
    public Task<List<TaskList>> GetAllAsync();

    public Task<TaskList?> GetByIdAsync(int id);

    public Task<TaskList> CreateAsync(TaskList taskList);

    public Task<TaskList> UpdateAsync(TaskList taskList);

    public Task<TaskList> SoftDelete(TaskList taskList);
}
