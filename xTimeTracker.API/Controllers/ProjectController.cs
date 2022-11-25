using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using xTimeTracker.API.Models;
using xTimeTracker.Core;
using xTimeTracker.Core.Services;
using xTimeTracker.DataAccess.MSSQL.Entities;

namespace xTimeTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(IProjectService projectService, IMapper mapper, ILogger<ProjectController> logger)
        {
            _projectService = projectService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectCreateRequest projectRequest)
        {
            try 
            { 
                var project = _mapper.Map<ProjectCreateRequest, Core.Project>(projectRequest);
                var result = await _projectService.CreateProject(project);

                _logger.LogInformation("post\n\tDateTime: {0}\n\tRequest: {1}\n\tResponse: {2} ", DateTime.Now, JsonSerializer.Serialize(projectRequest), result);

                if (!result)
                {
                    return StatusCode(417, "ExpectationFailed");
                }
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _projectService.GetProjects();
                _logger.LogInformation("get\n\tDateTime: {0}", DateTime.Now);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(417, $"ExpectationFailed. Message: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProjectUpdateRequest projectRequest)
        {
            try
            {
                var result = await _projectService.UpdateProject(
                    _mapper.Map<ProjectUpdateRequest, Core.Project>(projectRequest));

                _logger.LogInformation("put\n\tDateTime: {0}\n\tRequest: {1}\n\tResponse: {2} ", DateTime.Now, JsonSerializer.Serialize(projectRequest), result);

                if (!result)
                {
                    return StatusCode(417, "ExpectationFailed");
                }
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int projectId)
        {
            try
            {
                var result = await _projectService.DeleteProject(projectId);

                _logger.LogInformation("delete\n\tDateTime: {0}\n\tRequest: projectId = {1}\n\tResponse: {2} ", DateTime.Now, projectId, result);

                if (!result)
                {
                    return StatusCode(417, "ExpectationFailed");
                }
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("ProjectsWithTime")]
        public async Task<IActionResult> GetProjectsWithTime([FromQuery]GetProjectWithTimeRequest dates)
        {
            try 
            {
                var result = await _projectService.GetTimeForProjectsByDate(dates.Start, dates.End);
                if( result.Count() == 0)
                {
                    return NotFound();
                }
                _logger.LogInformation("delete\n\tDateTime: {0}\n\tRequest: startDate = {1} endDate = {2}\n\tResponse: {3} ", DateTime.Now, dates.Start, dates.End, result);

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

    }
}