using api.Dtos.Task;
using api.Mappers;
using api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/task")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly ITaskService _service;

    public TaskController(ITaskService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var tasks = await _service.GetAll();

            return Ok(tasks.Select(t => t.ToTaskDto()).ToList());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequestDto taskDto)
    {
        try
        {
            var task = await _service.Create(taskDto);

            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task.ToTaskDto());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        try
        {
            var task = await _service.GetByIdAsync(id);
            return Ok(task.ToTaskDto());
        } 
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateById([FromRoute] int id, [FromBody] UpdateTaskRequestDto taskDto) 
    {
        try
        {
            var task = await _service.Update(id, taskDto);
            return Ok(task.ToTaskDto());
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return Conflict(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteById([FromRoute] int id)
    {
        try
        {
            await _service.DeleteByIdAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        } 
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
