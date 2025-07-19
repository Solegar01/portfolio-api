namespace PortfolioApi.Services.ProfileService
{
    public interface IProfileService
    {
        Task<ProfileViewModel?> GetProfileAsync(int id);
        Task<ProfileViewModel> CreateProfileAsync(ProfileViewModel profile);
        Task<ProfileViewModel?> UpdateProfileAsync(int id, ProfileViewModel updatedProfile);
        Task<bool> DeleteProfileAsync(int id);
        Task<IEnumerable<ProfileViewModel>> GetAllProfilesAsync();
    }
}
