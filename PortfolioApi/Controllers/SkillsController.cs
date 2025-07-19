using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Models;
using PortfolioApi.Services.SkillService;

namespace PortfolioApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillService _skillService;

        public SkillsController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SkillViewModel>>> GetAll()
        {
            var skills = await _skillService.GetAllAsync();
            if (skills == null || !skills.Any())
                return NotFound(ApiResponseViewModel<string>.Fail("No skills found"));

            // Map the skills to SkillViewModel
            var skillViewModels = skills.Select(skill => new SkillViewModel
            {
                Id = skill.Id,
                Name = skill.Name,
                Level = skill.Level,
                Category = skill.Category
            });

            return Ok(ApiResponseViewModel<IEnumerable<SkillViewModel>>.Ok(skillViewModels));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SkillViewModel>> GetById(int id)
        {
            var skill = await _skillService.GetByIdAsync(id);
            if (skill == null)
                return NotFound(ApiResponseViewModel<string>.Fail("Skill not found"));
            var skillViewModel = new SkillViewModel
            {
                Id = skill.Id,
                Name = skill.Name,
                Level = skill.Level,
                Category = skill.Category
            };
            return Ok(ApiResponseViewModel<SkillViewModel>.Ok(skillViewModel));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<SkillViewModel>> Create(SkillViewModel skill)
        {
            if (skill == null)
                return BadRequest(ApiResponseViewModel<string>.Fail("Invalid skill data"));
            var createdSkill = await _skillService.CreateAsync(skill);
            if (createdSkill == null)
                return BadRequest(ApiResponseViewModel<string>.Fail("Skill creation failed"));
            return CreatedAtAction(nameof(GetById), new { id = createdSkill.Id }, ApiResponseViewModel<SkillViewModel>.Ok(createdSkill));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SkillViewModel updatedSkill)
        {
            if (updatedSkill == null)
                return BadRequest(ApiResponseViewModel<string>.Fail("Invalid skill data"));
            var existingSkill = await _skillService.GetByIdAsync(id);
            if (existingSkill == null)
                return NotFound(ApiResponseViewModel<string>.Fail("Skill not found"));
            var updated = await _skillService.UpdateAsync(id, updatedSkill);
            if (updated == null)
                return BadRequest(ApiResponseViewModel<string>.Fail("Skill update failed"));
            return Ok(ApiResponseViewModel<SkillViewModel>.Ok(updated));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingSkill = await _skillService.GetByIdAsync(id);
            if (existingSkill == null)
                return NotFound(ApiResponseViewModel<string>.Fail("Skill not found"));
            var deleted = await _skillService.DeleteAsync(id);
            if (!deleted)
                return BadRequest(ApiResponseViewModel<string>.Fail("Skill deletion failed"));
            return Ok(ApiResponseViewModel<string>.Ok("Skill deleted successfully"));
        }
    }
}
