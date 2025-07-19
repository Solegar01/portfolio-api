namespace PortfolioApi.Services.EducationService
{
    public interface IEducationService
    {
        Task<IEnumerable<EducationViewModel>> GetAllAsync();
        Task<EducationViewModel?> GetByIdAsync(int id);
        Task<EducationViewModel> CreateAsync(EducationViewModel education);
        Task<EducationViewModel?> UpdateAsync(int id, EducationViewModel updatedEducation);
        Task<bool> DeleteAsync(int id);
    }
}
