using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Models;
using PortfolioApi.Services.ContactMessageService;
using PortfolioApi.ViewModels;

namespace PortfolioApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IContactMessageService _contactMessageService;

        public ContactController(IContactMessageService contactMessageService)
        {
            _contactMessageService = contactMessageService;
        }

        [HttpPost("send")]
        public async Task<ActionResult> Send(ContactMessageViewModel message)
        {
            if (string.IsNullOrEmpty(message.Name) || string.IsNullOrEmpty(message.Email) || string.IsNullOrEmpty(message.Message))
            {
                return BadRequest(ApiResponseViewModel<string>.Fail("Name, email, and message are required"));
            }
            var contactMessage = new ContactMessageViewModel
            {
                Name = message.Name,
                Email = message.Email,
                Message = message.Message,
                CreatedAt = DateTime.UtcNow
            };
            await _contactMessageService.SendAsync(contactMessage);
            return Ok(ApiResponseViewModel<string>.Ok("Message sent successfully"));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactMessageViewModel>>> GetAll()
        {
            var messages = await _contactMessageService.GetAllAsync();
            if (messages == null || !messages.Any())
            {
                return NotFound(ApiResponseViewModel<string>.Fail("No messages found"));
            }
            return Ok(ApiResponseViewModel<IEnumerable<ContactMessageViewModel>>.Ok(messages));
        }
    }
}
