using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Models;

namespace PortfolioApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> Send(ContactMessage message)
        {
            if (string.IsNullOrWhiteSpace(message.Email) || string.IsNullOrWhiteSpace(message.Message))
                return BadRequest("Email and Message are required.");

            message.CreatedAt = DateTime.UtcNow;

            _context.ContactMessages.Add(message);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Thank you for contacting me!" });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactMessage>>> GetAll()
        {
            return await _context.ContactMessages
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
    }
}
