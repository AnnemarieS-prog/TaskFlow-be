using api.Dtos.TaskList;
using api.Mappers;
using api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/tasklist")]
    [ApiController]
    public class TaskListController : ControllerBase
    {
        private readonly ITaskListService _service;

        public TaskListController(ITaskListService service)
        {
            _service = service;
        }

        #region CRUD
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var taskLists = await _service.GetAllAsync();
            return Ok(taskLists.Select(t => t.ToTaskListPreviewDto()).ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskListRequestDto requestDto)
        {
            var taskList = await _service.Create(requestDto);
            return CreatedAtAction(nameof(GetById), new { id = taskList.Id }, taskList.ToTaskListPreviewDto());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id, [FromQuery] bool isPreview) 
        {
            try
            {
                var taskList = await _service.GetByIdAsync(id);

                return Ok(isPreview ? taskList.ToTaskListPreviewDto() : taskList.ToTaskListDto());
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
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTaskListRequestDto requestDto)
        {
            try
            {
                var taskList = await _service.Update(id, requestDto);
                return Ok(taskList.ToTaskListDto());
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
        public async Task<IActionResult> Delete([FromRoute] int id, [FromQuery] int targetId = 0)
        {
            try
            {
                var taskList = await _service.DeleteByIdAsync(id, targetId);
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
        #endregion

        #region interactions
        [HttpPost("{listId}/tasks/{taskId}")]
        public async Task<IActionResult> AddTaskToList([FromRoute] int listId, [FromRoute] int taskId)
        {
            try
            {
                var task = await _service.AddTaskToList(listId, taskId);
                return Ok(task.ToTaskDto());
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            } catch (InvalidOperationException e)
            {
                return Conflict(e.Message);
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{listId}/tasks/{taskId}")]
        public async Task<IActionResult> DeleteTaskFromList([FromRoute] int listId, [FromRoute] int taskId)
        {
            try
            {
                var task = await _service.RemoveTaskFromList(listId, taskId);
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
        #endregion
    }
}
