using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PortfolioApi.Data;
using PortfolioApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using PortfolioApi.Services.UserService;
using PortfolioApi.ViewModels;

namespace PortfolioApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config; // Added IConfiguration dependency

        public AuthController(IUserService userService, IConfiguration config) // Inject IConfiguration
        {
            _userService = userService;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserViewModel model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                return BadRequest(ApiResponseViewModel<string>.Fail("Email and password are required"));

            var existingUser = await _userService.GetByEmailAsync(model.Email);
            if (existingUser != null)
                return Conflict(ApiResponseViewModel<string>.Fail("User already exists"));

            var user = await _userService.RegisterAsync(model);
            if (user == null)
                return BadRequest(ApiResponseViewModel<string>.Fail("Registration failed"));

            return Ok(ApiResponseViewModel<string>.Ok("User registered successfully"));
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserViewModel model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                return BadRequest(ApiResponseViewModel<string>.Fail("Email and password are required"));

            try
            {
                var user = await _userService.LoginAsync(model);
                var token = GenerateJwtToken(user.Email); // Gunakan email langsung
                return Ok(ApiResponseViewModel<Object>.Ok(new { token }));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(ApiResponseViewModel<string>.Fail("Invalid email or password"));
            }
        }

        private string GenerateJwtToken(string email)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.NameIdentifier, email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"], // ✅ tambahkan Audience di appsettings jika perlu
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
