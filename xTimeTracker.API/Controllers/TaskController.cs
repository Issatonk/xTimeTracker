using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using xTimeTracker.API.Models;
using xTimeTracker.Core.Services;

namespace xTimeTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskController> _logger;

        public TaskController(ITaskService taskService, IMapper mapper, ILogger<TaskController> logger)
        {
            _taskService = taskService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskCreateRequest taskRequest)
        {
            var task = _mapper.Map<TaskCreateRequest, Core.Task>(taskRequest);
            var result = await _taskService.Create(task);

            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetByProject(int projectId)
        {
            var result = await _taskService.GetTasksByProject(projectId);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(TaskUpdateRequest taskRequest)
        {
            var task = _mapper.Map<TaskUpdateRequest, Core.Task>(taskRequest);
            var result = await _taskService.Update(task);

            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int taskId)
        {
            var result = await _taskService.Delete(taskId);
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}