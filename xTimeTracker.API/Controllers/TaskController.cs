using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
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
            _logger.LogInformation("post\n\tDateTime: {0}\n\tRequest: {1}\n\tResponse: {2} ", DateTime.Now, JsonSerializer.Serialize(taskRequest), result);
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
            _logger.LogInformation("get\n\tDateTime: {0}", DateTime.Now);
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

            _logger.LogInformation("put\n\tDateTime: {0}\n\tRequest: {1}\n\tResponse: {2} ", DateTime.Now, JsonSerializer.Serialize(taskRequest), result);


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

            _logger.LogInformation("delete\n\tDateTime: {0}\n\tRequest: projectId = {1}\n\tResponse: {2} ", DateTime.Now, taskId, result);

            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}