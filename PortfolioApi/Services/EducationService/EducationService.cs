using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Models;

namespace PortfolioApi.Services.EducationService
{
    public class EducationService: IEducationService
    {
        private readonly ApplicationDbContext _context;
        public EducationService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<EducationViewModel>> GetAllAsync()
        {
            return await _context.Educations
                .Select(e => new EducationViewModel
                {
                    Id = e.Id,
                    Institution = e.Institution,
                    Degree = e.Degree,
                    FieldOfStudy = e.FieldOfStudy,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                })
                .ToListAsync();
        }
        public async Task<EducationViewModel?> GetByIdAsync(int id)
        {
            var education = await _context.Educations.FindAsync(id);
            if (education == null) return null;
            return new EducationViewModel
            {
                Id = education.Id,
                Institution = education.Institution,
                Degree = education.Degree,
                FieldOfStudy = education.FieldOfStudy,
                StartDate = education.StartDate,
                EndDate = education.EndDate,
            };
        }
        public async Task<EducationViewModel> CreateAsync(EducationViewModel education)
        {
            var newEducation = new Education
            {
                Institution = education.Institution,
                Degree = education.Degree,
                FieldOfStudy = education.FieldOfStudy,
                StartDate = education.StartDate,
                EndDate = education.EndDate,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _context.Educations.Add(newEducation);
            await _context.SaveChangesAsync();
            education.Id = newEducation.Id; // Set the ID of the created education
            return education;
        }
        public async Task<EducationViewModel?> UpdateAsync(int id, EducationViewModel updatedEducation)
        {
            var existingEducation = await _context.Educations.FindAsync(id);
            if (existingEducation == null) return null;
            existingEducation.Institution = updatedEducation.Institution;
            existingEducation.Degree = updatedEducation.Degree;
            existingEducation.FieldOfStudy = updatedEducation.FieldOfStudy;
            existingEducation.StartDate = updatedEducation.StartDate;
            existingEducation.EndDate = updatedEducation.EndDate;
            await _context.SaveChangesAsync();
            return updatedEducation;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var education = await _context.Educations.FindAsync(id);
            if (education == null) return false;
            _context.Educations.Remove(education);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
