using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Models;
using PortfolioApi.Services.ProjectService;

namespace PortfolioApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _projectService.GetAllAsync();
            return Ok(ApiResponseViewModel<IEnumerable<ProjectViewModel>>.Ok(projects));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null)
            {
                return NotFound(ApiResponseViewModel<string>.Fail("Project not found"));
            }
            return Ok(ApiResponseViewModel<ProjectViewModel>.Ok(project));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProjectViewModel project)
        {
            if (project == null)
            {
                return BadRequest(ApiResponseViewModel<string>.Fail("Invalid project data"));
            }
            var createdProject = await _projectService.CreateAsync(project);
            return CreatedAtAction(nameof(GetById), new { id = createdProject.Id }, ApiResponseViewModel<ProjectViewModel>.Ok(createdProject));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProjectViewModel updatedProject)
        {
            if (updatedProject == null)
            {
                return BadRequest(ApiResponseViewModel<string>.Fail("Invalid project data"));
            }
            var existingProject = await _projectService.GetByIdAsync(id);
            if (existingProject == null)
            {
                return NotFound(ApiResponseViewModel<string>.Fail("Project not found"));
            }
            var project = await _projectService.UpdateAsync(id, updatedProject);
            return Ok(ApiResponseViewModel<ProjectViewModel>.Ok(project));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingProject = await _projectService.GetByIdAsync(id);
            if (existingProject == null)
            {
                return NotFound(ApiResponseViewModel<string>.Fail("Project not found"));
            }
            await _projectService.DeleteAsync(id);
            return Ok(ApiResponseViewModel<string>.Ok("Project deleted successfully"));
        }
    }
}
