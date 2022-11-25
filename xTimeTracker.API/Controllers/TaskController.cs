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
            try
            {
                var task = _mapper.Map<TaskCreateRequest, Core.Task>(taskRequest);
                var result = await _taskService.Create(task);
                if (!result)
                {
                    return StatusCode(417, "ExpectationFailed");
                }
                _logger.LogInformation("post\n\tDateTime: {0}\n\tRequest: {1}\n\tResponse: {2} ", DateTime.Now, JsonSerializer.Serialize(taskRequest), result);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetByProject(int projectId)
        {
            try
            {
                var result = await _taskService.GetTasksByProject(projectId);
                if (result == null)
                {
                    return NotFound();
                }
                _logger.LogInformation("get\n\tDateTime: {0}", DateTime.Now);
                return Ok(result);
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(TaskUpdateRequest taskRequest)
        {
            try
            {
                var task = _mapper.Map<TaskUpdateRequest, Core.Task>(taskRequest);
                var result = await _taskService.Update(task);            

                if (!result)
                {
                    return StatusCode(417, "ExpectationFailed");
                }
                _logger.LogInformation("put\n\tDateTime: {0}\n\tRequest: {1}\n\tResponse: {2} ", DateTime.Now, JsonSerializer.Serialize(taskRequest), result);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int taskId)
        {
            try
            {
                var result = await _taskService.Delete(taskId);

                if (!result)
                {
                    return StatusCode(417, "ExpectationFailed");
                }
                _logger.LogInformation("delete\n\tDateTime: {0}\n\tRequest: projectId = {1}\n\tResponse: {2} ", DateTime.Now, taskId, result);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}