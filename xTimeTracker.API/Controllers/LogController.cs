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
            try
            {
                var log = _mapper.Map<LogCreateRequest, Core.Log>(logRequest);
                var result = await _logService.Create(log);


                if (!result)
                {
                    return StatusCode(417, "ExpectationFailed");
                }
                _logger.LogInformation("post\n\tDateTime: {0}\n\tRequest: {1}\n\tResponse: {2} ", DateTime.Now, JsonSerializer.Serialize(logRequest), result);           
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("logs")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _logService.GetAll();

                if (result == null)
                {
                    return NotFound();
                }
                _logger.LogInformation("get\n\tDateTime: {0}", DateTime.Now);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(417, "ExpectationFailed");
            }
        }

        [HttpGet("tasklogs")]
        public async Task<IActionResult> Get(int taskId)
        {
            try
            {
                var result = await _logService.GetLogsByTask(taskId);

                if (result == null)
                {
                    return NotFound();
                }
                _logger.LogInformation("getByTask\n\tDateTime: {0}\n\ttaskId: {1}", DateTime.Now, taskId);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("intervallogs")]
        public async Task<IActionResult> Get(DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = await _logService.GetLogsByDateInterval(startDate, endDate);

                if (result == null)
                {
                    return NotFound();
                }
                _logger.LogInformation("getByDate\n\tDateTime: {0}\n\tstart: {1}\n\tend: {2}", DateTime.Now, startDate.Date, endDate.Date);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("logWithTaskAndProjectNames")]
        public async Task<IActionResult> GetWithNames()
        {
            try
            {
                var result = await _logService.GetLogsWithTaskNameAndProjectName();
                if(result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch
            {
                return StatusCode(417, "ExpectationFailed");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(LogUpdateRequest logRequest)
        {
            try
            {
                var log = _mapper.Map<LogUpdateRequest, Core.Log>(logRequest);
                var result = await _logService.Update(log);

                if (!result)
                {
                    return StatusCode(417, "ExpectationFailed");
                }

                _logger.LogInformation("put\n\tDateTime: {0}\n\tRequest: {1}\n\tResponse: {2} ", DateTime.Now, JsonSerializer.Serialize(logRequest), result);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int logId)
        {
            try
            {
                var result = await _logService.Delete(logId);

                if (!result)
                {
                    return StatusCode(417, "ExpectationFailed");
                }

                _logger.LogInformation("delete\n\tDateTime: {0}\n\tRequest: projectId = {1}\n\tResponse: {2} ", DateTime.Now, logId, result);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}