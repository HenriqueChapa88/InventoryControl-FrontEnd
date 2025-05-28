using InventoryControl.Core.Entities;

namespace InventoryControl.Core.Interfaces
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<Supplier?> GetByIdAsync(int id);
        Task AddAsync(Supplier entity);
        Task UpdateAsync(Supplier entity);
        Task DeleteAsync(Supplier entity);
        Task<bool> CNPJExistsAsync(string cnpj);
    }
}