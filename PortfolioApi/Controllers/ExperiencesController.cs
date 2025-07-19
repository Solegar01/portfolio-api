using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Models;
using PortfolioApi.Services.ExperienceService;

namespace PortfolioApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExperiencesController : ControllerBase
    {
        private readonly IExperienceService _experienceService;
        public ExperiencesController(IExperienceService experienceService)
        {
            _experienceService = experienceService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var experiences = await _experienceService.GetAllAsync();
            return Ok(ApiResponseViewModel<IEnumerable<ExperienceViewModel>>.Ok(experiences));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var experience = await _experienceService.GetByIdAsync(id);
            if (experience == null)
            {
                return NotFound(ApiResponseViewModel<string>.Fail("Experience not found"));
            }
            return Ok(ApiResponseViewModel<ExperienceViewModel>.Ok(experience));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExperienceViewModel experience)
        {
            if (experience == null)
            {
                return BadRequest(ApiResponseViewModel<string>.Fail("Invalid experience data"));
            }
            var createdExperience = await _experienceService.CreateAsync(experience);
            return CreatedAtAction(nameof(GetById), new { id = createdExperience.Id }, ApiResponseViewModel<ExperienceViewModel>.Ok(createdExperience));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ExperienceViewModel updatedExperience)
        {
            if (updatedExperience == null)
            {
                return BadRequest(ApiResponseViewModel<string>.Fail("Invalid experience data"));
            }
            var existingExperience = await _experienceService.GetByIdAsync(id);
            if (existingExperience == null)
            {
                return NotFound(ApiResponseViewModel<string>.Fail("Experience not found"));
            }
            var updated = await _experienceService.UpdateAsync(id, updatedExperience);
            return Ok(ApiResponseViewModel<ExperienceViewModel>.Ok(updated));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _experienceService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(ApiResponseViewModel<string>.Fail("Experience not found"));
            }
            return Ok(ApiResponseViewModel<string>.Ok("Experience deleted successfully"));
        }
    }
}
