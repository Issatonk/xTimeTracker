using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using xTimeTracker.API.Models;
using xTimeTracker.BusinessLogic;
using xTimeTracker.Core.Services;

namespace xTimeTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;
        private readonly IMapper _mapper;
        private readonly ILogger<LogController> _logger;

        public LogController(ILogService logService, IMapper mapper, ILogger<LogController> logger)
        {
            _logService = logService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create(LogCreateRequest logRequest)
        {
            var log = _mapper.Map<LogCreateRequest, Core.Log>(logRequest);
            var result = await _logService.Create(log);

            _logger.LogInformation("post\n\tDateTime: {0}\n\tRequest: {1}\n\tResponse: {2} ", DateTime.Now, JsonSerializer.Serialize(logRequest), result);

            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpGet("logs")]
        public async Task<IActionResult> Get()
        {
            var result = await _logService.GetAll();

            _logger.LogInformation("get\n\tDateTime: {0}", DateTime.Now);

            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpGet("tasklogs")]
        public async Task<IActionResult> Get(int taskId)
        {
            var result = await _logService.GetLogsByTask(taskId);

            _logger.LogInformation("getByTask\n\tDateTime: {0}\n\ttaskId: {1}", DateTime.Now, taskId);

            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpGet("intervallogs")]
        public async Task<IActionResult> Get(DateTime startDate, DateTime endDate)
        {
            var result = await _logService.GetLogsByDateInterval(startDate, endDate);

            _logger.LogInformation("getByDate\n\tDateTime: {0}\n\tstart: {1}\n\tend: {2}", DateTime.Now, startDate.Date, endDate.Date);

            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(LogUpdateRequest logRequest)
        {
            var log = _mapper.Map<LogUpdateRequest, Core.Log>(logRequest);
            var result = await _logService.Update(log);

            _logger.LogInformation("put\n\tDateTime: {0}\n\tRequest: {1}\n\tResponse: {2} ", DateTime.Now, JsonSerializer.Serialize(logRequest), result);


            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int logId)
        {
            var result = await _logService.Delete(logId);

            _logger.LogInformation("delete\n\tDateTime: {0}\n\tRequest: projectId = {1}\n\tResponse: {2} ", DateTime.Now, logId, result);


            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}