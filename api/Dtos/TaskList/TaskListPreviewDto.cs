namespace api.Dtos.TaskList
{
    public class TaskListPreviewDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public int OpenTasks { get; set; }
        public int CompletedTasks { get; set; }
    }
}
