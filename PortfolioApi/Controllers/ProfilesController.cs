using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Models;
using PortfolioApi.Services.ProfileService;

namespace PortfolioApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfilesController : ControllerBase
    {
        private readonly IProfileService _profileService;
        public ProfilesController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var profiles = await _profileService.GetAllProfilesAsync();
            return Ok(ApiResponseViewModel<IEnumerable<ProfileViewModel>>.Ok(profiles));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var profile = await _profileService.GetProfileAsync(id);
            if (profile == null)
            {
                return NotFound(ApiResponseViewModel<string>.Fail("Profile not found"));
            }
            return Ok(ApiResponseViewModel<ProfileViewModel>.Ok(profile));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProfileViewModel profile)
        {
            if (profile == null)
            {
                return BadRequest(ApiResponseViewModel<string>.Fail("Invalid profile data"));
            }
            var createdProfile = await _profileService.CreateProfileAsync(profile);
            return CreatedAtAction(nameof(GetById), new { id = createdProfile.Id }, ApiResponseViewModel<ProfileViewModel>.Ok(createdProfile));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProfileViewModel updatedProfile)
        {
            if (updatedProfile == null)
            {
                return BadRequest(ApiResponseViewModel<string>.Fail("Invalid profile data"));
            }
            var existingProfile = await _profileService.GetProfileAsync(id);
            if (existingProfile == null)
            {
                return NotFound(ApiResponseViewModel<string>.Fail("Profile not found"));
            }
            var updated = await _profileService.UpdateProfileAsync(id, updatedProfile);
            if (updated == null)
            {
                return BadRequest(ApiResponseViewModel<string>.Fail("Update failed"));
            }
            return Ok(ApiResponseViewModel<ProfileViewModel>.Ok(updated));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingProfile = await _profileService.GetProfileAsync(id);
            if (existingProfile == null)
            {
                return NotFound(ApiResponseViewModel<string>.Fail("Profile not found"));
            }
            var deleted = await _profileService.DeleteProfileAsync(id);
            if (!deleted)
            {
                return BadRequest(ApiResponseViewModel<string>.Fail("Delete failed"));
            }
            return Ok(ApiResponseViewModel<string>.Ok("Profile deleted successfully"));
        }
    }
}
