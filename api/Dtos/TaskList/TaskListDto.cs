using api.Dtos.Task;

namespace api.Dtos.TaskList;

public class TaskListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    
    public IEnumerable<TaskDto> OpenTasks { get; set; } = new List<TaskDto>();
    public IEnumerable<TaskDto> CompletedTasks { get; set; } = new List<TaskDto>();

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
