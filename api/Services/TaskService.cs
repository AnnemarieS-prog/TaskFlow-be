using api.Dtos.Task;
using api.Mappers;
using api.Models;
using api.Repositories;

namespace api.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;

    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<MyTask>> GetAll()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<MyTask> GetByIdAsync(int id)
    {
        var task = await _repository.GetByIdAsync(id);

        if (task == null)
            throw new KeyNotFoundException();
        
        return task;
    }

    public async Task<MyTask> Create(CreateTaskRequestDto dto)
    {
        var task = dto.ToTaskFromCreateDto();

        return await _repository.Create(task);
    }
    public async Task<MyTask> Update(int id, UpdateTaskRequestDto dto)
    {
        var task = await _repository.GetByIdAsync(id);

        if (task == null)
            throw new KeyNotFoundException("Task to update doesn't exist");

        if (task.IsCompleted)
            throw new InvalidOperationException("Can't update completed tasks");

        task.Title = dto.Title ?? task.Title;
        task.Description = dto.Description ?? task.Description;
        task.IsCompleted = dto.IsCompleted ?? task.IsCompleted;
        task.IsPrioritized = dto.IsPrioritized ?? task.IsPrioritized;

        return await _repository.Update(task);
    }

    public async Task<MyTask> DeleteByIdAsync(int id)
    {
        var task = await _repository.GetByIdAsync(id);

        if (task == null)
            throw new KeyNotFoundException("Task to delete doesn't exist");

        return await _repository.SoftDelete(task);
    }

    public async Task<List<MyTask>> MoveTasks(int targetId, List<MyTask> tasks, bool save)
    {
        var targetList = await _repository.GetByIdAsync(targetId);
        if (targetId != 0 && targetList == null)
            throw new KeyNotFoundException("TargetList to move tasks doesn't exist");

        tasks.ForEach(t => t.TaskListId = targetId == 0 ? null : targetId);

        var saveTasks = tasks.Select(t => _repository.Update(t, save));

        return tasks;
    }

    public async Task<List<MyTask>> DeleteTasks(List<MyTask> tasks, bool save)
    {
        var deleteTasks = tasks.Select(t => _repository.SoftDelete(t, save));
        await Task.WhenAll(deleteTasks);

        return tasks;
    }

    public async Task<MyTask> AddTaskToList(MyTask task, TaskList list)
    {
        if (task.TaskListId == list.Id)
            throw new InvalidOperationException("Task is already in this list");

        task.TaskListId = list.Id;
        return await _repository.Update(task);
    }

    public async Task<MyTask> RemoveTaskFromList(MyTask task, TaskList list)
    {
        if (task.TaskListId != list.Id)
            throw new InvalidOperationException("Task is not part of this list");

        task.TaskListId = null;
        return await _repository.Update(task);
    }

}
