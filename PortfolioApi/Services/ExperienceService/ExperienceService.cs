using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Models;

namespace PortfolioApi.Services.ExperienceService
{
    public class ExperienceService : IExperienceService
    {
        private readonly ApplicationDbContext _context;
        public ExperienceService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ExperienceViewModel>> GetAllAsync()
        {
            return await _context.Experiences
                .Select(e => new ExperienceViewModel
                {
                    Id = e.Id,
                    Company = e.Company,
                    Position = e.Position,
                    Description = e.Description,
                    IsCurrent = e.IsCurrent,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                })
                .ToListAsync();
        }
        public async Task<ExperienceViewModel?> GetByIdAsync(int id)
        {
            var experience = await _context.Experiences.FindAsync(id);
            if (experience == null) return null;
            return new ExperienceViewModel
            {
                Id = experience.Id,
                Company = experience.Company,
                Position = experience.Position,
                Description = experience.Description,
                IsCurrent = experience.IsCurrent,
                StartDate = experience.StartDate,
                EndDate = experience.EndDate,
            };
        }
        public async Task<ExperienceViewModel> CreateAsync(ExperienceViewModel experience)
        {
            var newExperience = new Experience
            {
                Company = experience.Company,
                Position = experience.Position,
                Description = experience.Description,
                IsCurrent = experience.IsCurrent,
                StartDate = experience.StartDate,
                EndDate = experience.EndDate,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _context.Experiences.Add(newExperience);
            await _context.SaveChangesAsync();
            experience.Id = newExperience.Id; // Set the ID of the created experience
            return experience;
        }
        public async Task<ExperienceViewModel?> UpdateAsync(int id, ExperienceViewModel updatedExperience)
        {
            var existingExperience = await _context.Experiences.FindAsync(id);
            if (existingExperience == null) return null;
            existingExperience.Company = updatedExperience.Company;
            existingExperience.Position = updatedExperience.Position;
            existingExperience.Description = updatedExperience.Description;
            existingExperience.IsCurrent = updatedExperience.IsCurrent;
            existingExperience.StartDate = updatedExperience.StartDate;
            existingExperience.EndDate = updatedExperience.EndDate;
            await _context.SaveChangesAsync();
            return updatedExperience;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var experience = await _context.Experiences.FindAsync(id);
            if (experience == null) return false;
            _context.Experiences.Remove(experience);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
