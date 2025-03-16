using api.Data;
using api.Dtos.Task;
using api.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/task")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly ApplicationDBContext _context;

    public TaskController(ApplicationDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var tasks = _context.Tasks
            .Where(t => t.DeletedAt == null)
            .Select(t => t.ToTaskDto())
            .ToList();

        return Ok(tasks);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateTaskRequestDto taskDto)
    {
        var taskModel = taskDto.ToTaskFromCreateDto();

        _context.Tasks.Add(taskModel);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetById), new {id = taskModel.Id }, taskModel.ToTaskDto());
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var task = _context.Tasks
            .Where(t => t.DeletedAt == null)
            .FirstOrDefault(t => t.Id == id);

        if (task == null) return NotFound();

        return Ok(task.ToTaskDto());
    }

    [HttpPut("{id}")]
    public IActionResult UpdateById([FromRoute] int id, [FromBody] UpdateTaskRequestDto taskDto) 
    {
        var task = _context.Tasks
            .Where(t => t.DeletedAt == null)
            .FirstOrDefault(t => t.Id == id);

        if (task == null) return NotFound();

        // TODO: post auth-impl => change to Forbid()
        if (task.IsCompleted) return Conflict(new { message = "You're trying to update a task that has already been completed!"});

        task.Title = taskDto.Title ?? task.Title;
        task.Description = taskDto.Description ?? task.Description;
        task.IsCompleted = taskDto.IsCompleted ?? task.IsCompleted;
        task.IsPrioritized = taskDto.IsPrioritized ?? task.IsPrioritized;

        var entry = _context.Entry(task);
        if (entry.Properties.Any(p => p.IsModified))
        {
            task.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();

            return Ok(task.ToTaskDto());
        } else
        {
            return BadRequest("No changes requested.");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteById([FromRoute] int id)
    {
        var task = _context.Tasks
            .Where(t => t.DeletedAt == null)
            .FirstOrDefault(t => t.Id == id);

        if (task == null) return NotFound();

        task.DeletedAt = DateTime.UtcNow;
        _context.SaveChanges();
        
        return NoContent();
    }
}
