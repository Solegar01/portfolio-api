using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Models;

namespace PortfolioApi.Services.ProjectService
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;
        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProjectViewModel>> GetAllAsync()
        {
            return await _context.Projects
                .Select(p => new ProjectViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Link = p.Link,
                    Technologies = p.Technologies,
                    Year = p.Year
                }).ToListAsync();
        }
        public async Task<ProjectViewModel?> GetByIdAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return null;
            return new ProjectViewModel
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                Technologies = project.Technologies,
                Link = project.Link,
                Year = project.Year,
            };
        }
        public async Task<ProjectViewModel> CreateAsync(ProjectViewModel project)
        {
            var newProject = new Project
            {
                Title = project.Title,
                Description = project.Description,
                Link = project.Link,
                Technologies = project.Technologies,
                Year = project.Year,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _context.Projects.Add(newProject);
            await _context.SaveChangesAsync();
            project.Id = newProject.Id;
            return project;
        }
        public async Task<ProjectViewModel?> UpdateAsync(int id, ProjectViewModel updatedProject)
        {
            var existingProject = await _context.Projects.FindAsync(id);
            if (existingProject == null) return null;
            existingProject.Title = updatedProject.Title;
            existingProject.Description = updatedProject.Description;
            existingProject.Link = updatedProject.Link;
            existingProject.Technologies = updatedProject.Technologies;
            existingProject.Year = updatedProject.Year;
            existingProject.UpdatedAt = DateTime.Now;
            _context.Entry(existingProject).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return new ProjectViewModel
            {
                Id = existingProject.Id,
                Title = existingProject.Title,
                Description = existingProject.Description,
                Technologies = existingProject.Technologies,
                Link = existingProject.Link,
                Year = existingProject.Year
            };
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return false;
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ProjectViewModel?> UpdateAsync(int id, Project updatedProject)
        {
            var existingProject = await _context.Projects.FindAsync(id);
            if (existingProject == null) return null;
            existingProject.Title = updatedProject.Title;
            existingProject.Description = updatedProject.Description;
            existingProject.Link = updatedProject.Link;
            existingProject.Technologies = updatedProject.Technologies;
            existingProject.Year = updatedProject.Year;
            existingProject.UpdatedAt = DateTime.Now;
            _context.Entry(existingProject).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return new ProjectViewModel
            {
                Id = existingProject.Id,
                Title = existingProject.Title,
                Description = existingProject.Description,
                Technologies = existingProject.Technologies,
                Link = existingProject.Link,
                Year = existingProject.Year
            };
        }
    }
}
