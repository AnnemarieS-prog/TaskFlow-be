namespace api.Dtos.Task
{
    public class UpdateTaskRequestDto
    {
        public string? Title { get; set; } = null;
        public string? Description { get; set; } = null;
        public bool? IsCompleted { get; set; } = null;
        public bool? IsPrioritized { get; set; } = null;
    }
}
