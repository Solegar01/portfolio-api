using PortfolioApi.Models;

namespace PortfolioApi.Services.ProjectService
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectViewModel>> GetAllAsync();
        Task<ProjectViewModel?> GetByIdAsync(int id);
        Task<ProjectViewModel> CreateAsync(ProjectViewModel project);
        Task<ProjectViewModel?> UpdateAsync(int id, ProjectViewModel updatedProject);
        Task<bool> DeleteAsync(int id);
    }
}
