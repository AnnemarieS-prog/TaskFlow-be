using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

public class TaskCollection
{
    public int Id { get; set; }
    public List<MyTask> Tasks { get; } = new List<MyTask>();

    public required string Title { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    [NotMapped]
    public IEnumerable<MyTask> OpenTasks => Tasks.Where(t => !t.IsCompleted);
    [NotMapped]
    public IEnumerable<MyTask> CompletedTasks => Tasks.Where(t => t.IsCompleted);

    public TaskCollection(string title)
    {
        Title = title;

        UpdatedAt = CreatedAt;
    }
}

