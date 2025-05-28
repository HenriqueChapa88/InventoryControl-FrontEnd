using InventoryControl.Core.Entities;

namespace InventoryControl.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid id);
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
    }
}