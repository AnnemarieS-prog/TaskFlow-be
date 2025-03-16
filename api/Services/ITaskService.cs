using api.Dtos.Task;
using api.Models;

namespace api.Services;

public interface ITaskService
{
    public Task<List<MyTask>> GetAll();

    public Task<MyTask> GetByIdAsync(int id);

    public Task<MyTask> Create(CreateTaskRequestDto dto);

    public Task<MyTask> Update(int id, UpdateTaskRequestDto dto);

    public Task<MyTask> DeleteByIdAsync(int id);

    public Task<List<MyTask>> MoveTasks(int targetId, List<MyTask> tasks, bool save = true);

    public Task<List<MyTask>> DeleteTasks(List<MyTask> tasks, bool save = true);

    public Task<MyTask> AddTaskToList(MyTask task, TaskList list);

    public Task<MyTask> RemoveTaskFromList(MyTask task, TaskList list);
}