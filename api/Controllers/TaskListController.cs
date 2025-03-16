using api.Data;
using api.Dtos.TaskList;
using api.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/tasklist")]
    [ApiController]
    public class TaskListController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public TaskListController(ApplicationDBContext context)
        {
            _context = context;
        }

        #region CRUD
        [HttpGet]
        public IActionResult GetAll()
        {
            var taskLists = _context.TaskLists
                .Where(l => l.DeletedAt == null)
                .Include(l => l.Tasks)
                .Select(l => l.ToTaskListPreviewDto())
                .ToList();

            return Ok(taskLists);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateTaskListRequestDto requestDto)
        {
            var taskListModel = requestDto.ToTaskListFromCreateDto();

            _context.TaskLists.Add(taskListModel);
            _context.SaveChanges();
            
            return CreatedAtAction(nameof(GetById), new { id = taskListModel.Id }, taskListModel.ToTaskListDto());
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id, [FromQuery] bool isPreview) 
        {
            var taskList = _context.TaskLists
                .Where(l => l.DeletedAt == null)
                .Include(l => l.Tasks)
                .FirstOrDefault(l => l.Id == id);

            if (taskList == null) return NotFound();

            return Ok(isPreview ? taskList.ToTaskListPreviewDto() : taskList.ToTaskListDto());
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateTaskListRequestDto requestDto)
        {
            var taskList = _context.TaskLists
                .Where(t => t.DeletedAt == null)
                .FirstOrDefault(t => t.Id == id);

            if (taskList == null) return NotFound();

            taskList.Title = requestDto.Title ?? taskList.Title;

            var entry = _context.Entry(taskList);
            if (entry.Properties.Any(p => p.IsModified))
            {
                taskList.UpdatedAt = DateTime.UtcNow;
                _context.SaveChanges();
                return Ok(taskList.ToTaskListDto());
            } else
            {
                return BadRequest("No changes requested.");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id, [FromQuery] int targetId = -1)
        {
            var taskList = _context.TaskLists
                .Where(l => l.DeletedAt == null)
                .Include(l => l.Tasks)
                .FirstOrDefault(l => l.Id == id);

            if (taskList == null) return NotFound("List to delete doesn't exist");

            switch (targetId)
            {
                case > -1:
                    var targetList = _context.TaskLists
                        .Where(tl => tl.DeletedAt == null)
                        .FirstOrDefault(tl => tl.Id == targetId);

                    if (targetList == null) return NotFound("TargetList to move tasks doesn't exist");

                    taskList.Tasks.ForEach(t => t.TaskListId = targetId);
                    break;

                case < -1:
                    taskList.Tasks.ForEach(t => t.DeletedAt = t.DeletedAt ?? DateTime.UtcNow);
                    break;

                default:
                    taskList.Tasks.ForEach(t => t.TaskListId = null);
                    break;
            }

            taskList.DeletedAt = DateTime.UtcNow;
            _context.SaveChanges();            
            
            return NoContent();
        }
        #endregion

        #region interactions
        [HttpPost("{listId}/tasks/{taskId}")]
        public IActionResult AddTaskToList([FromRoute] int listId, [FromRoute] int taskId)
        {
            var taskList = _context.TaskLists
                .Where(l => l.DeletedAt == null)
                .FirstOrDefault(l => l.Id == listId);
            var task = _context.Tasks
                .Where(t => t.DeletedAt == null)
                .FirstOrDefault(t => t.Id == taskId);
            
            if (taskList == null) return NotFound("TaskList doesn't exist");
            if (task == null) return NotFound("Task doesn't exist");

            if (task.TaskListId == listId) return Conflict("Task is already in this list.");

            task.TaskListId = listId;
            _context.SaveChanges();

            return Ok(task.ToTaskDto());
        }

        [HttpDelete("{listId}/tasks/{taskId}")]
        public IActionResult DeleteTaskFromList([FromRoute] int listId, [FromRoute] int taskId)
        {
            var taskList = _context.TaskLists
                .Where(l => l.DeletedAt == null)
                .FirstOrDefault(l => l.Id == listId);
            var task = _context.Tasks
                .Where(t => t.DeletedAt == null)
                .FirstOrDefault(t => t.Id == taskId);

            if (taskList == null || taskList.IsDeleted) return NotFound("TaskList doesn't exist");
            if (task == null || task.IsDeleted) return NotFound("Task doesn't exist");

            if (task.TaskListId != listId) return Conflict("Task is already not in this list");

            task.TaskListId = null;
            _context.SaveChanges();

            return Ok(task.ToTaskDto());
        }
        #endregion
    }
}
