using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Models;
using PortfolioApi.ViewModels;

namespace PortfolioApi.Services.UserService
{
    public class UserService: IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        public UserService(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task<UserViewModel> RegisterAsync(UserViewModel model)
        {
            if (_context.Users.Any(u => u.Email == model.Email))
                throw new Exception("User already exists");
            var user = new User
            {
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return model;
        }
        public async Task<UserViewModel> LoginAsync(UserViewModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");
            return model;
        }

        public async Task<UserViewModel?> GetByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return null;

            return new UserViewModel
            {
                Email = user.Email,
                Password = null // Jangan kembalikan password
            };
        }

    }
}
