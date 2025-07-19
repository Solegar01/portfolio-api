using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Models;
using PortfolioApi.Services.EducationService;

namespace PortfolioApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EducationsController : ControllerBase
    {
        private readonly IEducationService _educationService;
        public EducationsController(IEducationService educationService)
        {
            _educationService = educationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var educations = await _educationService.GetAllAsync();
            return Ok(ApiResponseViewModel<IEnumerable<EducationViewModel>>.Ok(educations));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var education = await _educationService.GetByIdAsync(id);
            if (education == null)
            {
                return NotFound(ApiResponseViewModel<string>.Fail("Education not found"));
            }
            return Ok(ApiResponseViewModel<EducationViewModel>.Ok(education));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EducationViewModel education)
        {
            if (education == null)
            {
                // Debug log
                using var reader = new StreamReader(Request.Body);
                var raw = await reader.ReadToEndAsync();
                Console.WriteLine("Raw JSON received: " + raw);

                return BadRequest(ApiResponseViewModel<string>.Fail("Invalid education data"));
            }

            var createdEducation = await _educationService.CreateAsync(education);
            return CreatedAtAction(nameof(GetById), new { id = createdEducation.Id }, ApiResponseViewModel<EducationViewModel>.Ok(createdEducation));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EducationViewModel updatedEducation)
        {
            if (updatedEducation == null)
            {
                return BadRequest(ApiResponseViewModel<string>.Fail("Invalid education data"));
            }
            var existingEducation = await _educationService.GetByIdAsync(id);
            if (existingEducation == null)
            {
                return NotFound(ApiResponseViewModel<string>.Fail("Education not found"));
            }
            var updated = await _educationService.UpdateAsync(id, updatedEducation);
            return Ok(ApiResponseViewModel<EducationViewModel>.Ok(updated));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingEducation = await _educationService.GetByIdAsync(id);
            if (existingEducation == null)
            {
                return NotFound(ApiResponseViewModel<string>.Fail("Education not found"));
            }
            await _educationService.DeleteAsync(id);
            return Ok(ApiResponseViewModel<string>.Ok("Education deleted successfully"));
        }
    }
}
