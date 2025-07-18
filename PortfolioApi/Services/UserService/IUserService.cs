using PortfolioApi.ViewModels;

namespace PortfolioApi.Services.UserService
{
    public interface IUserService
    {
        public Task<UserViewModel> LoginAsync(UserViewModel model);
        public Task<UserViewModel> RegisterAsync(UserViewModel model);
        public Task<UserViewModel> GetByEmailAsync(string email);
    }
}
