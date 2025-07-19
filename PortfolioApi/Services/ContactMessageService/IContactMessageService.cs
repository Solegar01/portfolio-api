using PortfolioApi.ViewModels;

namespace PortfolioApi.Services.ContactMessageService
{
    public interface IContactMessageService
    {
        Task<IEnumerable<ContactMessageViewModel>> GetAllAsync();
        Task<ContactMessageViewModel?> GetByIdAsync(int id);
        Task<ContactMessageViewModel> CreateAsync(ContactMessageViewModel contactMessage);
        Task<ContactMessageViewModel?> UpdateAsync(int id, ContactMessageViewModel updatedContactMessage);
        Task<bool> DeleteAsync(int id);
        Task<bool> SendAsync(ContactMessageViewModel contactMessage);
    }
}
