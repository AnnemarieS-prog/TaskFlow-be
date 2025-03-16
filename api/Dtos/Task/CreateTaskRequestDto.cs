namespace api.Dtos.Task;

public class CreateTaskRequestDto
{
    public string Title { get; protected set; }
    public string Description { get; protected set; }

    public CreateTaskRequestDto(string title, string description)
    {
        Title = title;
        Description = description;
    }
}
