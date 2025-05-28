using InventoryControl.Core.Entities;
using InventoryControl.Core.Interfaces;
using InventoryControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Infrastructure.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly AppDbContext _context;

        public SupplierRepository(AppDbContext context) => _context = context;

        public async Task AddAsync(Supplier entity)
        {
            await _context.Suppliers.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Supplier entity)
        {
            _context.Suppliers.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync() => 
            await _context.Suppliers.ToListAsync();

        public async Task<Supplier?> GetByIdAsync(int id) => 
            await _context.Suppliers.FindAsync(id);

        public async Task UpdateAsync(Supplier entity)
        {
            _context.Suppliers.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CNPJExistsAsync(string cnpj) => 
            await _context.Suppliers.AnyAsync(s => s.CNPJ == cnpj);
    }
}