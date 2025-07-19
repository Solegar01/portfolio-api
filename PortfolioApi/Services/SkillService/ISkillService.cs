namespace PortfolioApi.Services.SkillService
{
    public interface ISkillService
    {
        Task<IEnumerable<SkillViewModel>> GetAllAsync();
        Task<SkillViewModel?> GetByIdAsync(int id);
        Task<SkillViewModel> CreateAsync(SkillViewModel skill);
        Task<SkillViewModel?> UpdateAsync(int id, SkillViewModel updatedSkill);
        Task<bool> DeleteAsync(int id);
    }
}
