namespace api.Models;

// had to add prefix to circumvent confusion with 
// System.Threading.Tasks.Task
public class MyTask
{
    public int Id { get; set; }
    public int? TaskListId { get; set; }
    public TaskList? TaskList { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }

    public bool IsCompleted { get; set; } = false;
    public bool IsPrioritized { get; set; } = false;

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted => DeletedAt != null;

    public MyTask(string title, string? description)
    {
        Title = title;
        Description = description ?? string.Empty;

        UpdatedAt = CreatedAt;
    }
}