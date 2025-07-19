using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Models;
using PortfolioApi.ViewModels;
using System.Net.Mail;

namespace PortfolioApi.Services.ContactMessageService
{
    public class ContactMessageService : IContactMessageService
    {
        private readonly ApplicationDbContext _context;

        public ContactMessageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ContactMessageViewModel>> GetAllAsync()
        {
            return await _context.ContactMessages
                .Select(cm => new ContactMessageViewModel
                {
                    Id = cm.Id,
                    Name = cm.Name,
                    Email = cm.Email,
                    Message = cm.Message,
                    CreatedAt = cm.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<ContactMessageViewModel?> GetByIdAsync(int id)
        {
            var contactMessage = await _context.ContactMessages.FindAsync(id);
            if (contactMessage == null) return null;

            return new ContactMessageViewModel
            {
                Id = contactMessage.Id,
                Name = contactMessage.Name,
                Email = contactMessage.Email,
                Message = contactMessage.Message,
                CreatedAt = contactMessage.CreatedAt
            };
        }

        public async Task<ContactMessageViewModel> CreateAsync(ContactMessageViewModel contactMessageViewModel)
        {
            var contactMessage = new ContactMessage
            {
                Name = contactMessageViewModel.Name,
                Email = contactMessageViewModel.Email,
                Message = contactMessageViewModel.Message,
                CreatedAt = DateTime.UtcNow
            };

            _context.ContactMessages.Add(contactMessage);
            await _context.SaveChangesAsync();

            contactMessageViewModel.Id = contactMessage.Id;
            contactMessageViewModel.CreatedAt = contactMessage.CreatedAt;

            return contactMessageViewModel;
        }

        public async Task<ContactMessageViewModel?> UpdateAsync(int id, ContactMessageViewModel updatedContactMessage)
        {
            var contactMessage = await _context.ContactMessages.FindAsync(id);
            if (contactMessage == null) return null;

            contactMessage.Name = updatedContactMessage.Name;
            contactMessage.Email = updatedContactMessage.Email;
            contactMessage.Message = updatedContactMessage.Message;

            await _context.SaveChangesAsync();

            updatedContactMessage.Id = contactMessage.Id;
            updatedContactMessage.CreatedAt = contactMessage.CreatedAt;

            return updatedContactMessage;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message == null) return false;

            _context.ContactMessages.Remove(message);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SendAsync(ContactMessageViewModel contactMessage)
        {
            try
            {
                // Example implementation for sending an email
                using var smtpClient = new SmtpClient("smtp.example.com")
                {
                    Port = 587,
                    Credentials = new System.Net.NetworkCredential("your-email@example.com", "your-password"),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("your-email@example.com"),
                    Subject = "New Contact Message",
                    Body = $"Name: {contactMessage.Name}\nEmail: {contactMessage.Email}\nMessage: {contactMessage.Message}",
                    IsBodyHtml = false
                };

                mailMessage.To.Add("recipient@example.com");

                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch
            {
                // Log the exception or handle it as needed
                return false;
            }
        }
    }
}
