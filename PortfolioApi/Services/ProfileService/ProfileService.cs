using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Models;

namespace PortfolioApi.Services.ProfileService
{
    public class ProfileService : IProfileService
    {
        private readonly ApplicationDbContext _context;

        public ProfileService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProfileViewModel?> GetProfileAsync()
        {
            var profile = await _context.Profiles.FirstOrDefaultAsync();
            if (profile == null) return null;
            return new ProfileViewModel
            {
                Id = profile.Id,
                Name = profile.Name,
                Bio = profile.Bio,
                Email = profile.Email,
                Phone = profile.Phone,
                Location = profile.Location,
            };
        }

        public async Task<ProfileViewModel?> GetProfileAsync(int id)
        {
            var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.Id == id);
            if (profile == null) return null;
            return new ProfileViewModel
            {
                Id = profile.Id,
                Name = profile.Name,
                Bio = profile.Bio,
                Email = profile.Email,
                Phone = profile.Phone,
                Location = profile.Location,
            };
        }

        public async Task<ProfileViewModel> CreateProfileAsync(ProfileViewModel profile)
        {
            var newProfile = new Profile
            {
                Name = profile.Name,
                Bio = profile.Bio,
                Email = profile.Email,
                Phone = profile.Phone,
                Location = profile.Location,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Profiles.Add(newProfile);
            await _context.SaveChangesAsync();
            profile.Id = newProfile.Id;
            return profile;
        }

        public async Task<ProfileViewModel?> UpdateProfileAsync(int userId, ProfileViewModel updatedProfile)
        {
            var existingProfile = await _context.Profiles.FirstOrDefaultAsync(p => p.Id == userId);
            if (existingProfile == null) return null;

            existingProfile.Name = updatedProfile.Name;
            existingProfile.Bio = updatedProfile.Bio;
            existingProfile.Email = updatedProfile.Email;
            existingProfile.Phone = updatedProfile.Phone;
            existingProfile.Location = updatedProfile.Location;
            existingProfile.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return new ProfileViewModel
            {
                Id = existingProfile.Id,
                Name = existingProfile.Name,
                Bio = existingProfile.Bio,
                Email = existingProfile.Email,
                Phone = existingProfile.Phone,
                Location = existingProfile.Location,
            };
        }

        public async Task<bool> DeleteProfileAsync(int userId)
        {
            var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.Id == userId);
            if (profile == null) return false;

            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ProfileViewModel>> GetAllProfilesAsync()
        {
            return await _context.Profiles
                .Select(profile => new ProfileViewModel
                {
                    Id = profile.Id,
                    Name = profile.Name,
                    Bio = profile.Bio,
                    Email = profile.Email,
                    Phone = profile.Phone,
                    Location = profile.Location,
                })
                .ToListAsync();
        }
    }
}
