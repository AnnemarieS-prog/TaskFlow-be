using api.Dtos.TaskList;
using api.Models;

namespace api.Services;

public interface ITaskListService
{
    public Task<List<TaskList>> GetAllAsync();

    public Task<TaskList> GetByIdAsync(int id);

    public Task<TaskList> Create(CreateTaskListRequestDto dto);

    public Task<TaskList> Update(int id, UpdateTaskListRequestDto dto);

    public Task<TaskList> DeleteByIdAsync(int id, int targetId = 0);

    public Task<MyTask> AddTaskToList(int listId, int taskId);

    public Task<MyTask> RemoveTaskFromList(int listId, int taskId);
}
