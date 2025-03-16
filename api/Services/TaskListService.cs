using api.Dtos.TaskList;
using api.Mappers;
using api.Models;
using api.Repositories;

namespace api.Services;

public class TaskListService : ITaskListService
{
    private readonly ITaskListRepository _repository;
    private readonly ITaskService _taskService;

    public TaskListService(ITaskListRepository repository, ITaskService taskService)
    {
        _repository = repository;
        _taskService = taskService;
    }

    public async Task<List<TaskList>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<TaskList> GetByIdAsync(int id)
    {
        var taskList = await _repository.GetByIdAsync(id);

        if (taskList == null)
            throw new KeyNotFoundException();

        return taskList;
    }

    public async Task<TaskList> Create(CreateTaskListRequestDto dto)
    {
        var taskList = dto.ToTaskListFromCreateDto();

        return await _repository.CreateAsync(taskList);

    }

    public async Task<TaskList> Update(int id, UpdateTaskListRequestDto dto)
    {
        var taskList = await _repository.GetByIdAsync(id);

        if (taskList == null)
            throw new KeyNotFoundException("TaskList to update doesn't exist");

        taskList.Title = dto.Title ?? taskList.Title;

        return await _repository.UpdateAsync(taskList);
    }

    public async Task<TaskList> DeleteByIdAsync(int id, int targetId = 0)
    {
        var taskList = await _repository.GetByIdAsync(id);

        if (taskList == null)
            throw new KeyNotFoundException("TaskList to delete doesn't exist");

        if (targetId > -1)
        {
            await _taskService.MoveTasks(targetId, taskList.Tasks, false);
        } else
        {
            await _taskService.DeleteTasks(taskList.Tasks, false);
        }

        return await _repository.SoftDelete(taskList);
    }

    public async Task<MyTask> AddTaskToList(int listId, int taskId)
    {
        var taskList = await _repository.GetByIdAsync(listId);
        var task = await _taskService.GetByIdAsync(taskId);

        if (taskList == null)
            throw new KeyNotFoundException("TaskList to add task doesn't exist");

        return await _taskService.AddTaskToList(task, taskList);
    }

    public async Task<MyTask> RemoveTaskFromList(int listId, int taskId)
    {
        var taskList = await _repository.GetByIdAsync(listId);
        var task = await _taskService.GetByIdAsync(taskId);

        if (taskList == null)
            throw new KeyNotFoundException("TaskList to add task doesn't exist");

        return await _taskService.RemoveTaskFromList(task, taskList);
    }
}
