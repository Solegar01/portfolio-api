namespace PortfolioApi.Services.ExperienceService
{
    public interface IExperienceService
    {
        Task<IEnumerable<ExperienceViewModel>> GetAllAsync();
        Task<ExperienceViewModel?> GetByIdAsync(int id);
        Task<ExperienceViewModel> CreateAsync(ExperienceViewModel experience);
        Task<ExperienceViewModel?> UpdateAsync(int id, ExperienceViewModel updatedExperience);
        Task<bool> DeleteAsync(int id);
    }
}
