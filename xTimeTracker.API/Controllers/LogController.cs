using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}