using api.Dtos.Task;
using api.Models;

namespace api.Mappers;

public static class TaskMapper
{
    public static TaskDto ToTaskDto(this MyTask taskModel)
    {
        return new TaskDto
        {
            Id = taskModel.Id,
            Title = taskModel.Title,
            Description = taskModel.Description,
            IsCompleted = taskModel.IsCompleted,
            CreatedAt = taskModel.CreatedAt,
            UpdatedAt = taskModel.UpdatedAt,
            TaskListId = taskModel.TaskListId
        };
    }

    public static MyTask ToTaskFromCreateDto(this CreateTaskRequestDto taskDto)
    {
        return new MyTask(taskDto.Title, taskDto.Description);
    }
}
