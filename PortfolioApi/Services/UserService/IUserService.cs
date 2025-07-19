using PortfolioApi.ViewModels;

namespace PortfolioApi.Services.UserService
{
    public interface IUserService
    {
        Task<UserViewModel?> LoginAsync(UserViewModel model);
        Task<UserViewModel> RegisterAsync(UserViewModel model);
        Task<UserViewModel?> GetByEmailAsync(string email);
    }
}
