using api.Dtos.TaskList;
using api.Models;

namespace api.Mappers;

public static class TaskListMapper
{
    public static TaskListDto ToTaskListDto(this TaskList taskList)
    {
        return new TaskListDto
        {
            Id = taskList.Id,
            Title = taskList.Title,
            OpenTasks = taskList.OpenTasks.Select(t => t.ToTaskDto()),
            CompletedTasks = taskList.CompletedTasks.Select(t => t.ToTaskDto()),
            CreatedAt = taskList.CreatedAt,
            UpdatedAt = taskList.UpdatedAt
        };
    }

    public static TaskListPreviewDto ToTaskListPreviewDto(this TaskList taskList)
    {
        return new TaskListPreviewDto
        {
            Id = taskList.Id,
            Title = taskList.Title,
            OpenTasks = taskList.OpenTasks.Count(),
            CompletedTasks = taskList.CompletedTasks.Count()
        };
    }

    public static TaskList ToTaskListFromCreateDto(this CreateTaskListRequestDto requestDto)
    {
        return new TaskList(requestDto.Title);
    }
}
