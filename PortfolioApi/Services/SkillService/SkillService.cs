using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Models;

namespace PortfolioApi.Services.SkillService
{
    public class SkillService : ISkillService
    {
        private readonly ApplicationDbContext _context;
        public SkillService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SkillViewModel>> GetAllAsync()
        {
            return await _context.Skills
                .Select(s => new SkillViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Level = s.Level,
                    Category = s.Category
                })
                .ToListAsync();
        }
        public async Task<SkillViewModel?> GetByIdAsync(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null) return null;
            return new SkillViewModel
            {
                Id = skill.Id,
                Name = skill.Name,
                Level = skill.Level,
                Category = skill.Category
            };
        }
        public async Task<SkillViewModel> CreateAsync(SkillViewModel skill)
        {
            var newSkill = new Skill
            {
                Name = skill.Name,
                Level = skill.Level,
                Category = skill.Category,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _context.Skills.Add(newSkill);
            await _context.SaveChangesAsync();
            skill.Id = newSkill.Id; // Set the ID of the created skill
            return skill;
        }
        public async Task<SkillViewModel?> UpdateAsync(int id, SkillViewModel updatedSkill)
        {
            var existingSkill = await _context.Skills.FindAsync(id);
            if (existingSkill == null) return null;
            existingSkill.Name = updatedSkill.Name;
            existingSkill.Level = updatedSkill.Level;
            existingSkill.Category = updatedSkill.Category;
            await _context.SaveChangesAsync();
            return new SkillViewModel
            {
                Id = existingSkill.Id,
                Name = existingSkill.Name,
                Level = existingSkill.Level,
                Category = existingSkill.Category
            };
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null) return false;
            _context.Skills.Remove(skill);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
