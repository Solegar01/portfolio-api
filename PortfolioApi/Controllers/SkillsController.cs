using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Models;

namespace PortfolioApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SkillsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SkillsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/skills
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Skill>>> GetAll()
        {
            return await _context.Skills
                .OrderByDescending(s => s.Level)
                .ToListAsync();
        }

        // GET: api/skills/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Skill>> GetById(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
                return NotFound();
            return skill;
        }

        // POST: api/skills
        [HttpPost]
        public async Task<ActionResult<Skill>> Create(Skill skill)
        {
            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = skill.Id }, skill);
        }

        // PUT: api/skills/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Skill updatedSkill)
        {
            if (id != updatedSkill.Id)
                return BadRequest("ID mismatch");

            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
                return NotFound();

            skill.Name = updatedSkill.Name;
            skill.Level = updatedSkill.Level;
            skill.Category = updatedSkill.Category;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/skills/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
                return NotFound();

            _context.Skills.Remove(skill);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
