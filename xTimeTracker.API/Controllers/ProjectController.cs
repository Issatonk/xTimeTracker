﻿using AutoMapper;
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
            var project = _mapper.Map<ProjectCreateRequest, Core.Project>(projectRequest);
            var result = await _projectService.CreateProject(project);

            _logger.LogInformation("post\n\tDateTime: {0}\n\tRequest: {1}\n\tResponse: {2} ", DateTime.Now, JsonSerializer.Serialize(projectRequest), result);

            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _projectService.GetProjects();
            _logger.LogInformation("get\n\tDateTime: {0}", DateTime.Now);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProjectUpdateRequest projectRequest)
        {
            var result = await _projectService.UpdateProject(
                _mapper.Map<ProjectUpdateRequest, Core.Project>(projectRequest));

            _logger.LogInformation("put\n\tDateTime: {0}\n\tRequest: {1}\n\tResponse: {2} ", DateTime.Now, JsonSerializer.Serialize(projectRequest), result);

            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int projectId)
        {
            var result = await _projectService.DeleteProject(projectId);

            _logger.LogInformation("delete\n\tDateTime: {0}\n\tRequest: projectId = {1}\n\tResponse: {2} ", DateTime.Now, projectId, result);

            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet("ProjectsWithTime")]
        public async Task<IActionResult> GetProjectsWithTime([FromQuery]GetProjectWithTimeRequest dates)
        {
            _logger.LogInformation("delete\n\tDateTime: {0}\n\tRequest: startDate = {1} endDate = {2}\n\tResponse: {3} ", DateTime.Now, dates.Start, dates.End, result);

            var result = await _projectService.GetTimeForProjectsByDate(dates.Start, dates.End);

            return Ok(result);
        }

    }
}