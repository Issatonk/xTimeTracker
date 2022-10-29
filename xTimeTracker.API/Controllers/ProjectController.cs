using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using xTimeTracker.API.Models;
using xTimeTracker.Core;
using xTimeTracker.Core.Services;

namespace xTimeTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProjectController> _loger;

        public ProjectController(IProjectService projectService, IMapper mapper, ILogger<ProjectController> loger)
        {
            _projectService = projectService;
            _mapper = mapper;
            _loger = loger;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectCreateRequest projectRequest)
        {
            var project = _mapper.Map<ProjectCreateRequest, Core.Project>(projectRequest);
            var result = await _projectService.CreateProject(project);

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
            if(result == null)
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
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}