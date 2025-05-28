using InventoryControl.Core.Entities;

namespace InventoryControl.Core.Interfaces
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetAllAsync();
        Task<Item?> GetByIdAsync(int id);
        Task AddAsync(Item entity);
        Task UpdateAsync(Item entity);
        Task DeleteAsync(Item entity);
        Task<IEnumerable<Item>> GetBySupplierAsync(int supplierId);
        Task<Item?> GetBySkuAsync(string sku);
    }
}